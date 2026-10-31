using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.DictionaryDto.Requests;

public class UpdateDictionaryDto
{
    // [Required]
    // [MaxLength(150)]
    // public string Key { get; set; } = string.Empty;
    
    [Required]
    public string Value { get; set; }= string.Empty;

}