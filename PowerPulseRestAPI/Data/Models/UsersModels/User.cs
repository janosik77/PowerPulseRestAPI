using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data.Models.WorkSessionModels;

namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public long RoleId { get; set; }
        public long PersonId { get; set; }
        public DateTimeOffset? LastPasswordUpdate { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Role Role { get; set; } = null!;
        public Person Person { get; set; } = null!;
        public List<MaterialMovement> CreatedMaterialMovements { get; set; } = new();
        public List<ToolAssignment> CreatedToolAssignments { get; set; } = new();
        public List<VehicleIssue> VehicleIssuesReported { get; set; } = new();
        public List<Invoice> InvoicesCreated { get; set; } = new();
        public List<KnowledgeArticle> KnowledgeArticlesCreated { get;set; } = new();
        public List<KnowledgeArticleFavorite> KnowledgeFavorites { get; set; } = new();
        public List<KnowledgeArticleRead> KnowledgeReads { get; set; } = new();

    }
}
