using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.CustomerModels
{

    public class Customer
    {
        public long Id { get; set; }
        public CustomerStatus Status { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string TaxId { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public long ContactPersonId { get; set; }
        public long AddressId { get; set; }
        public bool IsDeleted { get; set; }

        public Address Address { get; set; } = null!;
        public Person ContactPerson { get; set; } = null!;
        public List<Invoice> Invoices { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
    }

}
