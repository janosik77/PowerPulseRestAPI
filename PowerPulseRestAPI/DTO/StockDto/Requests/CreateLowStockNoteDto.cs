using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.StockDto.Requests
{
    public class CreateLowStockNoteDto
    {
        [Required]
        public PurchasePriority Priority { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Note { get; set; } = null!;
    }
}
