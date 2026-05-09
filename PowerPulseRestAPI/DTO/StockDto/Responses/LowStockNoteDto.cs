using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.StockDto.Responses
{
    public class LowStockNoteDto
    {
        public long Id { get; set; }
        public PurchasePriority Priority { get; set; }
        public string Note { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public long CreatedByEmployeeId { get; set; }
        public string CreatedByEmployeeFullName { get; set; } = null!;

        public bool IsOwner { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
