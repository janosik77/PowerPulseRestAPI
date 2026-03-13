using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Request
{
    public sealed class CustomerUpdateRequest
    {
        public CustomerType CustomerType { get; set; }
        public CustomerStatus Status { get; set; }
        public string Name { get; set; } = null!;
        public string? TaxId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Note { get; set; }
    }
}
