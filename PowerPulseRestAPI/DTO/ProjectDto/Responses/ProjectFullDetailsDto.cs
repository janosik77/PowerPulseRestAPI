using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;
using PowerPulseRestAPI.DTO.MaterialDto.Responses;
using PowerPulseRestAPI.DTO.WorkSessionDto.Responses;


namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectFullDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? ContactPersonFullName { get; set; }
        public string? ContactPersonPhone { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string CustomerName { get; set; } = null!;
        public long? ResponsibleEmployeeId { get; set; }

        public List<ProjectTaskListItemDto> Tasks { get; set; } = new();
        public List<ProjectNoteListItemDto> Notes { get; set; } = new();
        public List<ProjectAttachmentListItemDto> Attachments { get; set; } = new();
        public List<WorkSessionListItemDto> WorkSessions { get; set; } = new();
        public List<ProjectMaterialBalanceDto> MaterialBalance { get; set; } = new();
        public List<ProjectAccessListItemDto> Accesses { get; set; } = new();
        public List<InvoiceListItemDto> Invoices { get; set; } = new();
    }
}
