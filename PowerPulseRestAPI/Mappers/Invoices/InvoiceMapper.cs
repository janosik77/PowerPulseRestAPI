using PowerPulseRestAPI.Data.Models.InvoiceModels;
using PowerPulseRestAPI.DTO.InvoiceDto.Responses;

namespace PowerPulseRestAPI.Mappers.Invoices
{
    public static class InvoiceMapper
    {
        public static InvoiceListItemDto ToListItemDto(this Invoice invoice)
        {
            return new InvoiceListItemDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Status = invoice.Status,
                ProjectId = invoice.ProjectId,
                ProjectName = invoice.Project?.Name ?? invoice.CustomerNameSnapshot ?? "-",
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.CompanyName ?? invoice.CustomerNameSnapshot ?? "-",
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                BillingPeriodStart = invoice.BillingPeriodStart,
                BillingPeriodEnd = invoice.BillingPeriodEnd,
                TotalAmount = invoice.TotalAmount,

            };
        }

        public static InvoiceDetailsDto ToDetailsDto(this Invoice invoice)
        {
            return new InvoiceDetailsDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Status = invoice.Status,
                ProjectId = invoice.ProjectId,
                ProjectName = invoice.Project?.Name ?? "-",
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.CompanyName ?? invoice.CustomerNameSnapshot ?? "-",
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                BillingPeriodStart = invoice.BillingPeriodStart,
                BillingPeriodEnd = invoice.BillingPeriodEnd,
                Currency = invoice.Currency,
                SubtotalAmount = invoice.SubtotalAmount,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                Note = invoice.Note,
                CustomerTaxIdSnapshot = invoice.CustomerTaxIdSnapshot,
                BillingAddressSnapshot = invoice.BillingAddressSnapshot,
                CreatedByUserName = invoice.CreatedByUser?.Login,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt,
                LaborItem = invoice.LaborItem == null
                    ? null
                    : new InvoiceLaborItemDto
                    {
                        Id = invoice.LaborItem.Id,
                        Quantity = invoice.LaborItem.Quantity,
                        Unit = invoice.LaborItem.Unit,
                        UnitPrice = invoice.LaborItem.UnitPrice,
                        TaxRate = invoice.LaborItem.TaxRate,
                        LineSubtotal = invoice.LaborItem.LineSubtotal,
                        LineTax = invoice.LaborItem.LineTax,
                        LineTotal = invoice.LaborItem.LineTotal
                    },
                MaterialItems = invoice.MaterialItems
                    .Select(x => new InvoiceMaterialItemDto
                    {
                        Id = x.Id,
                        MaterialId = x.MaterialId,
                        MaterialName = x.Material.Name,
                        Quantity = x.Quantity,
                        Unit = x.Unit,
                        UnitPrice = x.UnitPrice,
                        TaxRate = x.TaxRate,
                        LineSubtotal = x.LineSubtotal,
                        LineTax = x.LineTax,
                        LineTotal = x.LineTotal
                    })
                    .ToList()
            };
        }
    }
}
