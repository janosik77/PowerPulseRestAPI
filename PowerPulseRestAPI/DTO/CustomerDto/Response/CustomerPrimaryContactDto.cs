namespace PowerPulseRestAPI.DTO.CustomerDto.Response
{
    public sealed class CustomerPrimaryContactDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? RoleTitle { get; set; }
    }
}
