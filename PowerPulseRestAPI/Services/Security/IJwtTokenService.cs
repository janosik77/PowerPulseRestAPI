namespace PowerPulseRestAPI.Services.Security
{
    public interface IJwtTokenService
    {
        string GenerateToken(long userId, string login, string email, string role, long employeeId);
    }
}
