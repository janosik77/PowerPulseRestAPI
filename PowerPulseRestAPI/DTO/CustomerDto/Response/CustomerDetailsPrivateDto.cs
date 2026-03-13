using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class CustomerDetailsPrivateDto
    {
        public long Id { get; set; }
        public CustomerType CustomerType { get; set; }
        public CustomerStatus Status { get; set; }
        public string Name { get; set; } = null!;
        public string? TaxId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Note { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public List<CustomerContactDto> Contacts { get; set; } = new();
        public List<CustomerAddressDto> Addresses { get; set; } = new();
        public List<ProjectInCustomerDto> Projects { get; set; } = new();
        public List<CustomerNoteDto> Notes { get; set; } = new();
    }
}
