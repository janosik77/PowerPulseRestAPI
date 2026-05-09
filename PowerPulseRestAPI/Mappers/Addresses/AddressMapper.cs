using PowerPulseRestAPI.Data.Models.AddressModels;
using PowerPulseRestAPI.DTO.AddresesDto.Responses;

namespace PowerPulseRestAPI.Mappers.Addresses
{
    public static class AddressMapper
    {
        public static AddressDto ToDto(this Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                Country = address.Country,
                PostalCode = address.PostalCode,
                City = address.City,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
                ApartmentNumber = address.ApartmentNumber,
                AddressType = address.AddressType,
                FullText = address.FullText
            };
        }
    }
}
