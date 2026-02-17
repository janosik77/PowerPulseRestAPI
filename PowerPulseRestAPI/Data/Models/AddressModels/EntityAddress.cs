using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.Data.Models.AddressModels
{

    public class EntityAddress
    {
        public long Id { get; set; }
        public long AddressId { get; set; }
        public EntityType EntityType { get; set; }
        public long EntityId { get; set; }
        public AddressType AddressType { get; set; }
        public bool IsPrimary { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Address? Address { get; set; }
    }

}
