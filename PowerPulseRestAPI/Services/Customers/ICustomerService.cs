using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.DTO.CustomerDto.Request;
using PowerPulseRestAPI.DTO.CustomerDto.Response;

namespace PowerPulseRestAPI.Services.Customers
{
    public interface ICustomerService
    {
        // worker
        Task<IReadOnlyList<CustomerListItemDto>> GetListAsync(string? q, CustomerStatus? status, int skip, int take, CancellationToken ct);
        Task<CustomerPublicDetailsDto?> GetPublicDetailsAsync(long customerId, CancellationToken ct);

        // manager
        Task<long> CreateAsync(CustomerCreateRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateAsync(long customerId, CustomerUpdateRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DeleteAsync(long customerId, long managerUserId, CancellationToken ct);

        Task<CustomerDetailsPrivateDto?> GetManagerDetailsAsync(long customerId, CancellationToken ct);

        // contacts
        Task<long> AddContactAsync(long customerId, CustomerContactUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateContactAsync(long customerId, long contactId, CustomerContactUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DeleteContactAsync(long customerId, long contactId, long managerUserId, CancellationToken ct);

        // addresses (EntityAddress)
        Task<long> AddAddressAsync(long customerId, CustomerAddressUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> UpdateAddressAsync(long customerId, long entityAddressId, CustomerAddressUpsertRequest req, long managerUserId, CancellationToken ct);
        Task<bool> DeleteAddressAsync(long customerId, long entityAddressId, long managerUserId, CancellationToken ct);

        // notes
        Task<long> AddNoteAsync(long customerId, CustomerNoteCreateRequest req, long managerUserId, CancellationToken ct);

        // invoices
        Task<IReadOnlyList<CustomerInvoiceListItemDto>> GetInvoicesAsync(long customerId, InvoiceStatus? status, DateOnly? from, DateOnly? to, int skip, int take, CancellationToken ct);
    }
}
