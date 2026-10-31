using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.Data.Models.DictionaryModels;

public class Dictionary
{
    public long Id { get; set; }

    [Required] 
    [MaxLength(150)] 
    public string Key { get; set; } = string.Empty;
    
    [Required]
    public string Value { get; set; } = string.Empty;
    
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
}