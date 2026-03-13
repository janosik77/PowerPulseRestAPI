using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class ProjectInCustomerDto
    {
        public long ProjectId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public ProjectCustomerRelationshipType RelationshipType { get; set; }
        public bool? IsPrimaryOwner { get; set; }
    }
}
