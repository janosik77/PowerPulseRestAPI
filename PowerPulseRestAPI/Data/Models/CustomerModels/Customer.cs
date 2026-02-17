using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.CustomerModels
{

    public class Customer
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
        public List<CustomerContact> Contacts { get; set; } = new();
        public List<ProjectCustomer> ProjectCustomers { get; set; } = new();
        public List<CustomerNote> Notes { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();

    }

}
