using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.ToolDto.Requests
{
    public class ReturnToolDto
    {
        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
