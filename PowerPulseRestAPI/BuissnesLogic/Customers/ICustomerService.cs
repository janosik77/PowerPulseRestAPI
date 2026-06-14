using PowerPulseRestAPI.DTO.CustomerDto.Requests;
using PowerPulseRestAPI.DTO.CustomerDto.Responses;
using PowerPulseRestAPI.DTO.StatsDto.CustomerStats.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.Customers
{
    public interface ICustomerService
    {

        Task<CustomerDetailsDto> CreateAsync(
            CreateCustomerDto dto, 
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CustomerListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<CustomerDetailsDto> GetByIdAsync(
            long id, 
            CancellationToken cancellationToken = default);

        Task<CustomerDetailsDto> UpdateAsync(
            long id, 
            UpdateCustomerDto dto, 
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id, 
            CancellationToken cancellationToken = default);
        
        Task<CustomerStatsDto> GetStatsAsync(
            CancellationToken cancellationToken = default);
    }
}
