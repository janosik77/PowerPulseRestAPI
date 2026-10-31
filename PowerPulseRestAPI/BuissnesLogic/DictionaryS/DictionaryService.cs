using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.DictionaryDto.Requests;
using PowerPulseRestAPI.DTO.DictionaryDto.Responses;

using DictionaryEntity =
    PowerPulseRestAPI.Data.Models.DictionaryModels.Dictionary;

namespace PowerPulseRestAPI.BuissnesLogic.DictionaryS;

public class DictionaryService
    : IDictionaryService
{
    private readonly PowerPulseContext _dbContext;

    public DictionaryService(
        PowerPulseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<DictionaryDto>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        var entries =
            await _dbContext.Dictionaries
                .AsNoTracking()
                .OrderBy(x => x.Key)
                .ToListAsync(
                    cancellationToken);

        return entries
            .Select(Map)
            .ToList();
    }

    public async Task<DictionaryDto>
        GetByKeyAsync(
            string key,
            CancellationToken cancellationToken = default)
    {
        var normalizedKey =
            NormalizeKey(key);

        var entry =
            await _dbContext.Dictionaries
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Key == normalizedKey,
                    cancellationToken);

        if (entry == null)
        {
            throw new KeyNotFoundException(
                $"Dictionary entry '{normalizedKey}' was not found.");
        }

        return Map(entry);
    }

    public async Task<DictionaryOverviewDto>
        GetOverviewAsync(
            string? search = null,
            CancellationToken cancellationToken = default)
    {
        /*
         * Jedna mała tabela konfiguracyjna.
         * Pobieramy ją i całą logikę Content Managera
         * wykonujemy na BACKENDZIE.
         */
        var entries =
            await _dbContext.Dictionaries
                .AsNoTracking()
                .OrderBy(x => x.Key)
                .ToListAsync(
                    cancellationToken);

        /*
         * SUMMARY
         */
        var sectionKeys =
            entries
                .Select(x =>
                    GetSectionKey(x.Key))
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .ToList();

        var lastUpdated =
            entries.Count == 0
                ? null
                : entries.Max(x =>
                    (DateTimeOffset?)x.Updated);

        /*
         * SEARCH
         *
         * Front wysyła tylko tekst.
         * Sam niczego nie filtruje.
         */
        var visibleEntries =
            entries;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchValue =
                search.Trim();

            visibleEntries =
                entries
                    .Where(entry =>
                        entry.Key.Contains(
                            searchValue,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        entry.Value.Contains(
                            searchValue,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        GetDisplayName(entry.Key)
                            .Contains(
                                searchValue,
                                StringComparison.OrdinalIgnoreCase)
                        ||
                        GetSectionLabel(entry.Key)
                            .Contains(
                                searchValue,
                                StringComparison.OrdinalIgnoreCase))
                    .ToList();
        }

        /*
         * ALL
         */
        var sections =
            new List<DictionarySectionDto>
            {
                new()
                {
                    Key = "all",
                    Label = "All",
                    Count = entries.Count,

                    Items =
                        visibleEntries
                            .Select(MapOverviewItem)
                            .ToList()
                }
            };

        /*
         * SEKCJE DYNAMICZNIE Z PREFIXU KEY.
         *
         * Nic nie jest wpisane na sztywno.
         *
         * home.title
         * -> home / Home
         *
         * knowledgeBase.title
         * -> knowledgebase / Knowledge Base
         */
        var groupedSections =
            entries
                .GroupBy(entry =>
                    GetSectionKey(entry.Key))
                .Select(group =>
                {
                    var first =
                        group.First();

                    var sectionKey =
                        group.Key;

                    var label =
                        GetSectionLabel(
                            first.Key);

                    var visibleItems =
                        visibleEntries
                            .Where(entry =>
                                string.Equals(
                                    GetSectionKey(entry.Key),
                                    sectionKey,
                                    StringComparison.OrdinalIgnoreCase))
                            .Select(MapOverviewItem)
                            .ToList();

                    return new DictionarySectionDto
                    {
                        Key =
                            sectionKey,

                        Label =
                            label,

                        Count =
                            group.Count(),

                        Items =
                            visibleItems
                    };
                })
                .OrderBy(x => x.Label)
                .ToList();

        sections.AddRange(
            groupedSections);

        return new DictionaryOverviewDto
        {
            Summary =
                new DictionaryOverviewSummaryDto
                {
                    TotalEntries =
                        entries.Count,

                    TotalSections =
                        sectionKeys.Count,

                    LastUpdatedAt =
                        lastUpdated,

                    LastUpdatedLabel =
                        lastUpdated.HasValue
                            ? FormatDate(
                                lastUpdated.Value)
                            : "—"
                },

            Sections =
                sections
        };
    }

    public async Task<DictionaryDto>
        CreateAsync(
            CreateDictionaryDto dto,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var key =
            NormalizeKey(dto.Key);

        var value =
            NormalizeValue(dto.Value);

        var exists =
            await _dbContext.Dictionaries
                .AnyAsync(
                    x => x.Key == key,
                    cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                $"Dictionary entry with key '{key}' already exists.");
        }

        var now =
            DateTimeOffset.UtcNow;

        var entry =
            new DictionaryEntity
            {
                Key = key,
                Value = value,
                Created = now,
                Updated = now
            };

        _dbContext.Dictionaries.Add(
            entry);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return Map(entry);
    }

    public async Task<DictionaryDto>
        UpdateAsync(
            string key,
            UpdateDictionaryDto dto,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var normalizedKey =
            NormalizeKey(key);

        var entry =
            await _dbContext.Dictionaries
                .FirstOrDefaultAsync(
                    x =>
                        x.Key ==
                        normalizedKey,
                    cancellationToken);

        if (entry == null)
        {
            throw new KeyNotFoundException(
                $"Dictionary entry '{normalizedKey}' was not found.");
        }

        entry.Value =
            NormalizeValue(dto.Value);

        entry.Updated =
            DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return Map(entry);
    }

    public async Task DeleteAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var normalizedKey =
            NormalizeKey(key);

        var entry =
            await _dbContext.Dictionaries
                .FirstOrDefaultAsync(
                    x =>
                        x.Key ==
                        normalizedKey,
                    cancellationToken);

        if (entry == null)
        {
            throw new KeyNotFoundException(
                $"Dictionary entry '{normalizedKey}' was not found.");
        }

        _dbContext.Dictionaries.Remove(
            entry);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    private static DictionaryDto Map(
        DictionaryEntity entry)
    {
        return new DictionaryDto
        {
            Id = entry.Id,
            Key = entry.Key,
            Value = entry.Value,
            Created = entry.Created,
            Updated = entry.Updated
        };
    }

    private static DictionaryOverviewItemDto
        MapOverviewItem(
            DictionaryEntity entry)
    {
        return new DictionaryOverviewItemDto
        {
            Id =
                entry.Id,

            Key =
                entry.Key,

            Value =
                entry.Value,

            DisplayName =
                GetDisplayName(
                    entry.Key),

            SectionKey =
                GetSectionKey(
                    entry.Key),

            SectionLabel =
                GetSectionLabel(
                    entry.Key),

            Created =
                entry.Created,

            Updated =
                entry.Updated,

            UpdatedLabel =
                FormatDate(
                    entry.Updated)
        };
    }

    private static string NormalizeKey(
        string? key)
    {
        var value =
            key?.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                "Dictionary key is required.");
        }

        if (value.Length > 150)
        {
            throw new InvalidOperationException(
                "Dictionary key cannot exceed 150 characters.");
        }

        return value;
    }

    private static string NormalizeValue(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                "Dictionary value is required.");
        }

        return value.Trim();
    }

    private static string GetSectionPrefix(
        string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return "general";
        }

        var separatorIndex =
            key.IndexOf('.');

        if (separatorIndex <= 0)
        {
            return "general";
        }

        return key
            .Substring(
                0,
                separatorIndex)
            .Trim();
    }

    private static string GetSectionKey(
        string key)
    {
        return GetSectionPrefix(key)
            .ToLowerInvariant();
    }

    private static string GetSectionLabel(
        string key)
    {
        return Humanize(
            GetSectionPrefix(key));
    }

    private static string GetDisplayName(
        string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return "Entry";
        }

        var separatorIndex =
            key.IndexOf('.');

        if (
            separatorIndex < 0 ||
            separatorIndex ==
            key.Length - 1
        )
        {
            return Humanize(key);
        }

        var contentKey =
            key.Substring(
                separatorIndex + 1);

        return Humanize(
            contentKey);
    }

    private static string Humanize(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "General";
        }

        var result =
            value
                .Replace(".", " ")
                .Replace("_", " ")
                .Replace("-", " ");

        result =
            Regex.Replace(
                result,
                "([a-z0-9])([A-Z])",
                "$1 $2");

        result =
            Regex.Replace(
                result,
                "\\s+",
                " ")
            .Trim();

        if (string.IsNullOrWhiteSpace(result))
        {
            return "General";
        }

        return char.ToUpperInvariant(
                   result[0]) +
               result.Substring(1);
    }

    private static string FormatDate(
        DateTimeOffset value)
    {
        return value
            .ToLocalTime()
            .ToString(
                "dd MMM yyyy",
                CultureInfo.InvariantCulture);
    }
}