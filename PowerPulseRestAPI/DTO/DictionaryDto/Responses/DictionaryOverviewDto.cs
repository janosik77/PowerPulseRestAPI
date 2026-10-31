namespace PowerPulseRestAPI.DTO.DictionaryDto.Responses;

public class DictionaryOverviewDto
{
    public DictionaryOverviewSummaryDto Summary { get; set; }
        = new();

    public IReadOnlyList<DictionarySectionDto> Sections { get; set; }
        = Array.Empty<DictionarySectionDto>();
}

public class DictionaryOverviewSummaryDto
{
    public int TotalEntries { get; set; }

    public int TotalSections { get; set; }

    public DateTimeOffset? LastUpdatedAt { get; set; }

    public string LastUpdatedLabel { get; set; } = "—";
}

public class DictionarySectionDto
{
    public string Key { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public int Count { get; set; }

    public IReadOnlyList<DictionaryOverviewItemDto> Items { get; set; }
        = Array.Empty<DictionaryOverviewItemDto>();
}

public class DictionaryOverviewItemDto
{
    public long Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string SectionKey { get; set; } = string.Empty;

    public string SectionLabel { get; set; } = string.Empty;

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Updated { get; set; }

    public string UpdatedLabel { get; set; } = string.Empty;
}