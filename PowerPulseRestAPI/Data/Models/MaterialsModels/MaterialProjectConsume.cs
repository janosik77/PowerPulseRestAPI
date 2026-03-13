using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{
    public class MaterialProjectConsume
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }
        public long MaterialId { get; set; }

        public decimal PreviousQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal ConsumedQuantity { get; set; }

        public string Unit { get; set; } = null!;

        public Guid? InventoryBatchId { get; set; }

        public long? InvoiceId { get; set; }
        public DateTimeOffset? InvoicedAt { get; set; }

        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project Project { get; set; } = null!;
        public Material Material { get; set; } = null!;
        public Invoice? Invoice { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
