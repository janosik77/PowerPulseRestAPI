namespace PowerPulseRestAPI.Data.Models.UsersModels
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<User> Users { get; set; } = new();
    }
}
