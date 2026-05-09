using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public class VehicleAssignmentListItemToEmployeeDto
    {
        public long Id { get; set; }

        [Required]
        public long VehicleId { get; set; }

        [Required, MaxLength(100)]
        public string VehicleDisplayName { get; set; } = null!;

        [Required]
        public string VehiclePlateNumber { get; set; } = null!;

        [Required]
        public DateTimeOffset AssignedAt { get; set; }

        public DateTimeOffset? ReturnedAt { get; set; }

        public bool IsOpen => ReturnedAt == null;
    }
}
