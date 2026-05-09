using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.DTO.ToolDto.Requests;
using PowerPulseRestAPI.DTO.ToolDto.Responses;
using System;

namespace PowerPulseRestAPI.Services.ToolsS
{
    public class ToolCategoryService : IToolCategoryService
    {
        private readonly PowerPulseContext _dbContext;

        public ToolCategoryService(PowerPulseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> CreateAsync(
            CreateToolCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var exists = await _dbContext.ToolCategories
                .AnyAsync(x => x.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Tool category with name '{dto.Name}' already exists.");
            }

            if (dto.ParentId.HasValue)
            {
                var parentExists = await _dbContext.ToolCategories
                    .AnyAsync(x => x.Id == dto.ParentId.Value, cancellationToken);

                if (!parentExists)
                {
                    throw new KeyNotFoundException(
                        $"Parent tool category with id '{dto.ParentId.Value}' was not found.");
                }
            }

            var category = new ToolCategory
            {
                Name = dto.Name,
                ParentId = dto.ParentId,
                Description = dto.Description,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _dbContext.ToolCategories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }

        public async Task<IReadOnlyList<SelectOptionToolCategoryDto>> GetSelectOptionsAsync(
            CancellationToken cancellationToken = default)
        {
            var categories = await _dbContext.ToolCategories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new SelectOptionToolCategoryDto
                {
                    Value = x.Id,
                    Label = x.ParentId == null
                        ? x.Name
                        : $"{x.Parent!.Name} / {x.Name}"
                })
                .ToListAsync(cancellationToken);

            return categories;
        }

        public async Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.ToolCategories
                .Include(x => x.Tools)
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category is null)
            {
                throw new KeyNotFoundException(
                    $"Tool category with id '{id}' was not found.");
            }

            //if (category.Tools.Any())
            //{
            //    throw new InvalidOperationException(
            //        "Cannot delete tool category because it is assigned to existing tools.");
            //}

            if (category.Children.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete tool category because it has child categories.");
            }

            _dbContext.ToolCategories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
