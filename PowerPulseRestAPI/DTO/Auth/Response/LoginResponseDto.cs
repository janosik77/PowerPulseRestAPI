namespace PowerPulseRestAPI.DTO.Auth.Response
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public object? User { get; set; }
    }
}
