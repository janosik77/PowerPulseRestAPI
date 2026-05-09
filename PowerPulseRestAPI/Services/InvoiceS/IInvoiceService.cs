using PowerPulseRestAPI.DTO.InvoiceDto.Requests;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;

namespace PowerPulseRestAPI.Services.InvoiceS
{
    public interface IInvoiceService
    {
        Task<InvoiceDetailsDto> CreateAsync(
           CreateInvoiceDto dto,
           long createdByUserId,
           CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InvoiceListItemDto>> GetListAsync(
            CancellationToken cancellationToken = default);

        Task<InvoiceDetailsDto> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InvoiceMaterialSelectOptionDto>> GetMaterialSelectOptionsAsync(
            long projectId,
            DateOnly billingPeriodStart,
            DateOnly billingPeriodEnd,
            CancellationToken cancellationToken = default);

        //Task<InvoiceStatusSelectOptionDto> GetStatusSelectOptionAsync(
        //    CancellationToken cancellationToken = default);

        Task<InvoiceDetailsDto> UpdateAsync(
            long id,
            UpdateInvoiceDto dto,
            CancellationToken cancellationToken = default);

        Task<InvoiceDetailsDto> UpdateStatusAsync(
            long id,
            UpdateInvoiceStatusDto dto,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
