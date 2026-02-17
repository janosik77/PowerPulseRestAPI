using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;

namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{
    public class BillingRate
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }     // wymagane
        public string Name { get; set; } = null!;
        public decimal HourlyRate { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }

        public Project? Project { get; set; }
    }

}
