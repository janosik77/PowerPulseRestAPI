using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.CustomerModels
{
    public class CustomerPerson
    {
        public long CustomerId { get; set; }
        public long PersonId { get; set; }

        public CustomerContactRole ContactRole { get; set; }
        public bool IsPrimary { get; set; }

        public Customer Customer { get; set; } = null!;
        public Person Person { get; set; } = null!;
    }
}
