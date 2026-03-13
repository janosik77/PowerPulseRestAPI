using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.MaterialDto.Request
{
    public class CommitMaterialTransferRequest
    {
        public MaterialTransferEndpointType SourceType { get; set; }
        public long? SourceProjectId { get; set; }

        public MaterialTransferEndpointType TargetType { get; set; }
        public long? TargetProjectId { get; set; }

        public string? Note { get; set; }

        public List<CommitMaterialTransferItemRequest> Items { get; set; } = new();
    }
}
