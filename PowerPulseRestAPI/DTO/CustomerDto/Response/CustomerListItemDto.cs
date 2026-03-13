using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class CustomerListItemDto
    {
        public long Id { get; set; }
        public CustomerType CustomerType { get; set; }
        public CustomerStatus Status { get; set; }
        public string Name { get; set; } = null!;
        public string? TaxId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public CustomerPrimaryContactDto? PrimaryContact { get; set; }

        public int ProjectsCount { get; set; }
        public int ActiveProjectsCount { get; set; }
        public ProjectInCustomerDto? CurrentProject { get; set; } // opcjonalnie: np. ostatni/aktywny
    }
}
