using PowerPulseRestAPI.DTO.InvoiceDto.Requests;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;

namespace PowerPulseRestAPI.BuissnesLogic.InvoiceS
{
    public interface IInvoiceService
    {
        Task<InvoiceStatsDto> GetStatsAsync(
            CancellationToken cancellationToken = default);
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
            long? invoiceId = null,
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
        
        Task<IReadOnlyList<InvoiceWorkSessionSelectOptionDto>>
            GetWorkSessionSelectOptionsAsync(
                long projectId,
                DateOnly billingPeriodStart,
                DateOnly billingPeriodEnd,
                long? invoiceId = null,
                CancellationToken cancellationToken = default);
    }
}
