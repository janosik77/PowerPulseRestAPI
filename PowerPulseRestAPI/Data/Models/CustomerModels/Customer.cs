using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
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

        public List<CustomerNote> Notes { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
    }

}
