using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.DTO.EmployeeDto.Responses;

namespace PowerPulseRestAPI.Services.Employees
{
    public interface IEmployeeService
    {
        Task<EmployeeDetailPrivateDto> CreateAsync(
            CreateEmployeeDto dto,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SelectOptionsEmployeeDto>> GetSelectOptionsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EmployeeListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<EmployeeDetailsPublicDto> GetPublicByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<EmployeeDetailPrivateDto> GetPrivateByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<EmployeeDetailPrivateDto> UpdateAsync(
            long id,
            UpdateEmployeeDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
