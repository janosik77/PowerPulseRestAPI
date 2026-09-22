using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Models.TextControlModels;
using PowerPulseRestAPI.DTO.TextControlDto.Requests;
using PowerPulseRestAPI.DTO.TextControlDto.Responses;
using PowerPulseRestAPI.Mappers.TextTemplates;
using PowerPulseRestAPI.BuissnesLogic.TextTemplateS;

namespace PowerPulseRestAPI.BuissnesLogic
{
    public class TextTemplateService : ITextTemplateService
    {
        private readonly PowerPulseContext _dbContext;

        public TextTemplateService(PowerPulseContext db)
        {
            _dbContext = db;
        }

        public async Task<TextTemplateDto> CreateAsync(
            CreateTextTemplateDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var exists = await _dbContext.TextTemplates
                .AnyAsync(x => x.Key == dto.Key && x.Channel == dto.Channel, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Text template with key '{dto.Key}' and channel '{dto.Channel}' already exists.");
            }

            var now = DateTimeOffset.UtcNow;

            var template = new TextTemplate
            {
                Key = dto.Key,
                Channel = dto.Channel,
                TitleTemplate = dto.TitleTemplate,
                BodyTemplate = dto.BodyTemplate,
                IsActive = dto.IsActive,
                CreatedAt = now,
                UpdatedAt = now
            };

            _dbContext.TextTemplates.Add(template);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return template.ToDto();
        }

        public async Task<IReadOnlyList<TextTemplateListItemDto>> GetListAsync(
            ContentTemplateFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.TextTemplates
                .AsNoTracking()
                .AsQueryable();

            query = ApplyFilters(query, filter);

            var templates = await query
                .OrderBy(t => t.Key)
                .ThenBy(t => t.Id)
                .ToListAsync(cancellationToken);

            return templates
                .Select(t => new TextTemplateListItemDto
                {
                    Id = t.Id,
                    Key = t.Key,
                    Channel = t.Channel,
                    TitleTemplate = t.TitleTemplate,
                    BodyTemplate = t.BodyTemplate,
                    IsActive = t.IsActive,
                })
                .ToList();
        }
        
        public async Task<ContentTemplateStatsDto> GetStatsAsync(
            ContentTemplateFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.TextTemplates
                .AsNoTracking()
                .AsQueryable();

            query = ApplyFilters(query, filter);

            var templates = await query
                .Select(t => new
                {
                    t.Id,
                    t.Key,
                    t.IsActive
                })
                .ToListAsync(cancellationToken);

            return new ContentTemplateStatsDto
            {
                Summary = new ContentTemplateStatsSummaryDto
                {
                    TotalTemplates = templates.Count,
                    ActiveTemplates = templates.Count(t => t.IsActive),
                    InactiveTemplates = templates.Count(t => !t.IsActive),
                }
            };
        }

        public async Task<TextTemplateDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var template = await _dbContext.TextTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (template is null)
            {
                throw new KeyNotFoundException($"Text template with id '{id}' was not found.");
            }

            return template.ToDto();
        }

        public async Task<TextTemplateDto> UpdateAsync(
            long id,
            UpdateTextTemplateDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var template = await _dbContext.TextTemplates
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (template is null)
            {
                throw new KeyNotFoundException($"Text template with id '{id}' was not found.");
            }

            var exists = await _dbContext.TextTemplates
                .AnyAsync(
                    x => x.Id != id &&
                         x.Key == dto.Key &&
                         x.Channel == dto.Channel,
                    cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Text template with key '{dto.Key}' and channel '{dto.Channel}' already exists.");
            }

            template.Key = dto.Key;
            template.Channel = dto.Channel;
            template.TitleTemplate = dto.TitleTemplate;
            template.BodyTemplate = dto.BodyTemplate;
            template.IsActive = dto.IsActive;
            template.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return template.ToDto();
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var template = await _dbContext.TextTemplates
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (template is null)
            {
                throw new KeyNotFoundException($"Text template with id '{id}' was not found.");
            }

            _dbContext.TextTemplates.Remove(template);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TextTemplateDto> SetActiveAsync(
            long id,
            bool isActive,
            CancellationToken cancellationToken = default)
        {
            var template = await _dbContext.TextTemplates
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (template is null)
            {
                throw new KeyNotFoundException($"Text template with id '{id}' was not found.");
            }

            template.IsActive = isActive;
            template.UpdatedAt = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return template.ToDto();
        }

        public async Task<IReadOnlyList<TextTemplateOptionDto>> GetOptionsAsync(
            CancellationToken cancellationToken = default)
        {
            var templates = await _dbContext.TextTemplates
                .AsNoTracking()
                .Where(x => x.IsActive && !string.IsNullOrWhiteSpace(x.Key))
                .OrderBy(x => x.Key)
                .ToListAsync(cancellationToken);

            var options = templates
                .Select(x =>
                {
                    var key = x.Key.Trim();
                    var keyPrefix = key.Split('.', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? key;

                    var label = TemplateLabels.TryGetValue(keyPrefix, out var mappedLabel)
                        ? mappedLabel
                        : keyPrefix;

                    return new TextTemplateOptionDto
                    {
                        Value = key,
                        Label = label
                    };
                })
                .GroupBy(x => x.Value)
                .Select(g => g.First())
                .ToList();

            return options;
        }

        private static readonly Dictionary<string, string> TemplateLabels = new(StringComparer.OrdinalIgnoreCase)
        {
            ["home"] = "Home",
            ["customer"] = "Customers",
            ["employee"] = "Employees",
            ["projects"] = "Projects",
            ["workSessions"] = "Work Sessions",
            ["contentManager"] = "Content Manager",
            ["knowledgeBase"] = "Knowledge Base",
            ["vehicles"] = "Vehicles",
            ["materialsStorage"] = "Materials Storage",
            ["toolsStorage"] = "Tools Storage"
        };
        
        private IQueryable<TextTemplate> ApplyFilters(
            IQueryable<TextTemplate> query,
            ContentTemplateFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();
                var likeSearch = $"%{search}%";

                query = query.Where(t =>
                    EF.Functions.Like(t.Key, likeSearch) ||
                    EF.Functions.Like(t.TitleTemplate ?? string.Empty, likeSearch) ||
                    EF.Functions.Like(t.BodyTemplate, likeSearch));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(t => t.IsActive == filter.IsActive.Value);
            }

            return query;
        }
    }
}