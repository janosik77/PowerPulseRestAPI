using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.DTO.CustomerDto.Responses;
using PowerPulseRestAPI.DTO.PersonDto.Responses;
using PowerPulseRestAPI.DTO.ProjectDto.Responses;
using PowerPulseRestAPI.Mappers.Addresses;

namespace PowerPulseRestAPI.Mappers.Customers
{
    public static class CustomerMapper
    {
        public static CustomerListItemDto ToListItemDto(this Customer customer)
        {
            return new CustomerListItemDto
            {
                Id = customer.Id,
                Status = customer.Status,
                CompanyName = customer.CompanyName,
                PhoneNumber = customer.PhoneNumber,
                TaxId = customer.TaxId,
                AvatarUrl = customer.AvatarUrl,
                ContactPersonFullName = $"{customer.ContactPerson.FirstName} {customer.ContactPerson.LastName}",
                CreatedAt = customer.CreatedAt
            };
        }

        public static CustomerDetailsDto ToDetailsDto(this Customer customer)
        {
            return new CustomerDetailsDto
            {
                Id = customer.Id,
                Status = customer.Status,
                CompanyName = customer.CompanyName,
                PhoneNumber = customer.PhoneNumber,
                TaxId = customer.TaxId,
                AvatarUrl = customer.AvatarUrl,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt,
                Address = customer.Address.ToDto(),
                ContactPerson = customer.ContactPerson.ToCustomerContactPersonDto(),
                Projects = customer.Projects
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new CustomerProjectListItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = x.Status
                    })
                    .ToList()
            };
        }

        public static CustomerContactPersonDto ToCustomerContactPersonDto(this Person person)
        {
            return new CustomerContactPersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Phone = person.Phone,
                Email = person.Email,
                AvatarUrl = person.AvatarUrl,
                Address = person.Address is null ? null : person.Address.ToDto()
            };
        }
    }
}
