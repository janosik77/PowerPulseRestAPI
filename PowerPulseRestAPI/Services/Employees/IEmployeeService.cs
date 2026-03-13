using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.DTO.EmployeeDto.Requests;
using PowerPulseRestAPI.DTO.EmployeeDto.Responses;

namespace PowerPulseRestAPI.Services.Employees
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeCardDto>> GetCardsAsync(CancellationToken ct);

        Task<EmployeeDetailsPublicDto?> GetPublicDetailsAsync(long employeeId, CancellationToken ct);
        Task<EmployeeDetailsPrivateDto?> GetPrivateDetailsAsync(long employeeId, CancellationToken ct);

        Task<long?> GetEmployeeUserIdAsync(long employeeId, CancellationToken ct);

        Task<IReadOnlyList<EmployeeLeaveDto>> GetLeavesAsync(long employeeId, DateOnly from, DateOnly to, CancellationToken ct);

        // Manager: CRUD
        Task<long> CreateAsync(EmployeeCreateRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateAsync(long employeeId, EmployeeUpdateRequest req, CancellationToken ct);
        Task<bool> TerminateAsync(long employeeId, EmployeeTerminateRequest req, CancellationToken ct);

        // Vehicles
        Task<bool> AssignVehicleAsync(long employeeId, EmployeeAssignVehicleRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DetachCurrentVehicleAsync(long employeeId, long managerUserId, CancellationToken ct);

        // Tools
        Task<IReadOnlyList<EmployeeToolInDetailsDto>?> GetToolsAsync(long employeeId, CancellationToken ct);
        Task<bool> AssignToolAsync(long employeeId, EmployeeAssignToolRequest req, long managerUserId, CancellationToken ct);
        Task<int> ReturnAllToolsAsync(long employeeId, long managerUserId, CancellationToken ct);
        Task<bool> ReturnMyToolAsync(long toolAssignmentId, long userId, CancellationToken ct);
        // Sessions
        Task<bool> ForceEndActiveWorkSessionAsync(long employeeId, long managerUserId, CancellationToken ct);

        // Addresses
        Task<IReadOnlyList<EmployeeAddressDto>> GetAddressesAsync(long employeeId, CancellationToken ct);
        Task<long> AddAddressAsync(long employeeId, EmployeeAddressUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateAddressAsync(long employeeId, long entityAddressId, EmployeeAddressUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DeleteAddressAsync(long employeeId, long entityAddressId, long managerUserId, CancellationToken ct);

        // Bank accounts
        Task<IReadOnlyList<EmployeeBankAccountDto>> GetBankAccountsAsync(long employeeId, CancellationToken ct);
        Task<long> AddBankAccountAsync(long employeeId, EmployeeBankAccountUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateBankAccountAsync(long employeeId, long bankAccountId, EmployeeBankAccountUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DeleteBankAccountAsync(long employeeId, long bankAccountId, long managerUserId, CancellationToken ct);
        Task<Employee?> GetByIdAsync(long id, CancellationToken ct);
    }
}
