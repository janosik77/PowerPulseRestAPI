namespace PowerPulseRestAPI.DTO.UsersDto.Requests
{
    public class CreateCustomerPersonDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string Email { get; set; } = null!;
    }
}
