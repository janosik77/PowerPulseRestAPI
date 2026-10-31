namespace PowerPulseRestAPI.DTO.DictionaryDto.Responses;

public class DictionaryDto
{
    public long Id { get; set; }
    
    public string Key { get; set; }
    public string Value { get; set; }
    
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
}