using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class ServiceOrder
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public PurchasePriority Priority { get; set; }
        public long RequestedByUserId { get; set; }
        public DateTimeOffset RequestedAt { get; set; }
        public ServiceOrderApprovalStatus ApprovalStatus { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public string? ServiceProviderName { get; set; }
        public string? ServiceProviderPhone { get; set; }
        public DateTimeOffset? AppointmentAt { get; set; }
        public ServiceOrderStatus Status { get; set; }
        public int? MileageAtService { get; set; }
        public string? FinalDescription { get; set; }
        public decimal? TotalCost { get; set; }
        public string Currency { get; set; } = "PLN";
        public string? InvoiceNumber { get; set; }
        public long? CompletedByUserId { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
        public User? RequestedByUser { get; set; }
        public User? ApprovedByUser { get; set; }
        public User? CompletedByUser { get; set; }
        public List<ServiceOrderHistory> History { get; set; } = new();

    }

}
