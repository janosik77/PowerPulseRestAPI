using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.BuissnesLogic.Security;
using PowerPulseRestAPI.BuissnesLogic.Uploads;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.HomeDto.Responses;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.HomeS
{
    public class HomeService : IHomeService
    {
        private readonly PowerPulseContext _dbContext;

        public HomeService(PowerPulseContext db)
        {
            _dbContext = db;
        }

        public async Task<DashboardAnalyticsDto> GetDashboardAsync(
            long employeeId,
            CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.Now;;

            var sessions = await _dbContext.WorkSessions
                .AsNoTracking()
                .Include(s => s.Project)
                    .ThenInclude(s => s.Customer)
                .Where(s => s.EmployeeId == employeeId)
                .ToListAsync(cancellationToken);

            var activeSession = sessions
                .OrderByDescending(s => s.StartedAt)
                .FirstOrDefault(s => s.Status == 0 || s.EndedAt == null);

            var currentDayWorkTimeMilliseconds =
                CalculateCurrentDayWorkTimeMilliseconds(sessions, now);

            var monthlyActivity =
                CalculateCurrentMonthActivity(sessions, now);

            var assignedTasks =
                await GetAssignedTasksAsync(employeeId, cancellationToken);

            var assignedVehicle =
                await GetAssignedVehicleAsync(employeeId, cancellationToken);

            return new DashboardAnalyticsDto
            {
                Summary = new DashboardSummaryDto
                {
                    CurrentDayWorkTimeMilliseconds = currentDayWorkTimeMilliseconds,
                    CurrentDayWorkTimeLabel = FormatDuration(currentDayWorkTimeMilliseconds),
                    AttendanceDaysThisMonth = monthlyActivity.WorkedDays,
                    AssignedTasksCount = assignedTasks.Count,
                    ActiveSessionStatus = activeSession == null ? "Not started" : "Running"
                },

                MonthlyActivity = monthlyActivity,

                ActiveWorkSession = activeSession == null
                    ? null
                    : MapWorkSession(activeSession),

                AssignedTasks = assignedTasks,

                Vehicle = assignedVehicle,

                EmptyStates = new DashboardEmptyStatesDto
                {
                    Tasks = new DashboardEmptyStateDto
                    {
                        IsEmpty = assignedTasks.Count == 0,
                        Message = assignedTasks.Count == 0
                            ? await GetTextTemplateAsync(
                                "home.tasks.noTask",
                                cancellationToken)
                            : string.Empty
                    },

                    Vehicle = new DashboardEmptyStateDto
                    {
                        IsEmpty = assignedVehicle == null,
                        Message = assignedVehicle == null
                            ? await GetTextTemplateAsync(
                                "home.vehicle.notAssigned", 
                                cancellationToken)
                            : string.Empty
                    },

                    ActiveSession = new DashboardEmptyStateDto
                    {
                        IsEmpty = activeSession == null,
                        Message = activeSession == null
                            ? await GetTextTemplateAsync(
                                "home.activeWork",
                                cancellationToken)
                            : string.Empty
                    }
                }
            };
        }

        private static long CalculateCurrentDayWorkTimeMilliseconds(
            IEnumerable<dynamic> sessions,
            DateTimeOffset now)
        {
            var dayStart = new DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                0,
                0,
                0,
                now.Offset);

            var dayEnd = dayStart.AddDays(1);

            long totalMilliseconds = 0;

            foreach (var session in sessions)
            {
                if (session.StartedAt == null)
                {
                    continue;
                }

                DateTimeOffset start = session.StartedAt;
                DateTimeOffset end = session.EndedAt ?? now;

                var effectiveStart = start > dayStart ? start : dayStart;
                var effectiveEnd = end < dayEnd ? end : dayEnd;

                if (effectiveEnd > effectiveStart)
                {
                    totalMilliseconds +=
                        (long)(effectiveEnd - effectiveStart).TotalMilliseconds;
                }
            }

            return totalMilliseconds;
        }

        private static CurrentMonthActivityDto CalculateCurrentMonthActivity(
            IEnumerable<dynamic> sessions,
            DateTimeOffset now)
        {
            var monthStart = new DateTimeOffset(
                now.Year,
                now.Month,
                1,
                0,
                0,
                0,
                now.Offset);

            var monthEnd = monthStart.AddMonths(1);

            var monthSessions = sessions
                .Where(session =>
                {
                    if (session.StartedAt == null)
                    {
                        return false;
                    }

                    DateTimeOffset start = session.StartedAt;
                    DateTimeOffset end = session.EndedAt ?? now;

                    return start < monthEnd && end >= monthStart;
                })
                .ToList();

            long totalMilliseconds = 0;
            var workedDays = new HashSet<DateOnly>();

            foreach (var session in monthSessions)
            {
                DateTimeOffset start = session.StartedAt;
                DateTimeOffset end = session.EndedAt ?? now;

                var effectiveStart = start > monthStart ? start : monthStart;
                var effectiveEnd = end < monthEnd ? end : monthEnd;

                if (effectiveEnd <= effectiveStart)
                {
                    continue;
                }

                totalMilliseconds +=
                    (long)(effectiveEnd - effectiveStart).TotalMilliseconds;

                workedDays.Add(DateOnly.FromDateTime(effectiveStart.Date));
            }

            var totalHours = totalMilliseconds / 3_600_000d;

            var avgHoursPerDay = workedDays.Count > 0
                ? totalHours / workedDays.Count
                : 0;

            return new CurrentMonthActivityDto
            {
                Sessions = monthSessions.Count,
                WorkedDays = workedDays.Count,
                TotalMilliseconds = totalMilliseconds,
                TotalHours = totalHours,
                AvgHoursPerDay = avgHoursPerDay,
                TotalDurationLabel = FormatDuration(totalMilliseconds),
                AvgHoursPerDayLabel = $"{avgHoursPerDay:F1}h"
            };
        }

        private async Task<List<EmployeeTaskListItemDto>> GetAssignedTasksAsync(
            long employeeId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.ProjectTasks
                .AsNoTracking()
                .Where(t =>
                    t.AssignedToEmployeeId == employeeId)
                .OrderByDescending(t => t.Id)
                .Take(6)
                .Select(t => new EmployeeTaskListItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Priority = t.Priority,
                    ProjectName = t.Project.Name
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<VehicleListItemDto?> GetAssignedVehicleAsync(
            long employeeId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Vehicles
                .AsNoTracking()
                .Where(v => v.Assignments.Any(a =>
                    a.EmployeeId == employeeId
                    && a.ReturnedAt == null
                ))
                .Select(v => new VehicleListItemDto
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    PlateNumber = v.PlateNumber,
                    Year = v.Year,
                    Vin = v.Vin,
                    Status = v.Status,
                    Url = v.Url
                    
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        private static EmployeeWorkSessionsListItemDto MapWorkSession(dynamic session)
        { 
            return new EmployeeWorkSessionsListItemDto
            {
                Id = session.Id,
                ProjectId = session.ProjectId,
                ProjectCode = session.Project.Code,
                ProjectName = session.Project.Name, 
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                Status = session.Status,
                CustomerName = session.Project.Customer.CompanyName,
            };
        }

        private static string FormatDuration(long milliseconds)
        {
            if (milliseconds <= 0)
            {
                return "0h 0m";
            }

            var totalMinutes = milliseconds / 60_000;
            var hours = totalMinutes / 60;
            var minutes = totalMinutes % 60;

            return $"{hours}h {minutes}m";
        }

        private async Task<string> GetTextTemplateAsync(
            string key,
            CancellationToken cancellationToken)
        {
            var value = await _dbContext.TextTemplates
                .AsNoTracking()
                .Where(t =>
                    t.Key == key &&
                    t.Channel == TextTemplateChannel.UI &&
                    t.IsActive)
                .Select(t => t.BodyTemplate)
                .FirstOrDefaultAsync(cancellationToken);

            return value;
        }
    }
}