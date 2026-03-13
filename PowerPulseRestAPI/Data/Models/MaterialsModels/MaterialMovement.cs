using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.MaterialsModels
{


    public class MaterialMovement
        {
            public long Id { get; set; }
            public long MaterialId { get; set; }
            public MaterialMovementType MovementType { get; set; }
            public long? ProjectId { get; set; }
            public Guid OperationId { get; set; }
            public decimal Quantity { get; set; }
            public string Unit { get; set; } = null!;                   
            public string? Note { get; set; }
            public DateTimeOffset OccurredAt { get; set; }

            public long CreatedByUserId { get; set; }
            public DateTimeOffset CreatedAt { get; set; }

            public Material Material { get; set; } = null!;           
            public Project? Project { get; set; }
            public User CreatedByUser { get; set; } = null!;
        }
    
}
