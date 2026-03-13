namespace PowerPulseRestAPI.DTO.CustomerDto.Request
{
    public class CustomerContactUpsertRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? RoleTitle { get; set; }
        public bool IsPrimary { get; set; }
        public string? Note { get; set; }
    }
}
