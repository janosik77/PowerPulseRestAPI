using PowerPulseRestAPI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PowerPulseRestAPI.DTO.WorkSessionDto.Responses
{
    public class WorkSessionListItemDto
    {
        public long Id { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = null!;

        [Required]
        public long ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; } = null!;

        [Required]
        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? EndedAt { get; set; }

        [Required]
        [EnumDataType(typeof(WorkSessionStatus))]
        public WorkSessionStatus Status { get; set; }

        [MaxLength(2000)]
        public string? Note { get; set; }

        public long? InvoiceId { get; set; }
        public DateTimeOffset? InvoicedAt { get; set; }
    }
}
