using PowerPulseRestAPI.Data.Enums;


namespace PowerPulseRestAPI.DTO.ProjectDto.Responses
{
    public class ProjectFullDetailsDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CustomerName { get; set; } = null!;
        public long? ResponsibleEmployeeId { get; set; }

        public List<ProjectTaskListItemDto> Tasks { get; set; } = new();
        public List<ProjectNoteListItemDto> Notes { get; set; } = new();
        public List<ProjectAttachmentListItemDto> Attachments { get; set; } = new();
        public List<WorkSessionDto> WorkSessions { get; set; } = new();
        public List<MaterialProjectBalanceListItemDto> MaterialBalance { get; set; } = new();
        public List<ProjectAccessListItemDto> Accesses { get; set; } = new();
        public List<InvoiceDto> Invoices { get; set; } = new();
    }
}
