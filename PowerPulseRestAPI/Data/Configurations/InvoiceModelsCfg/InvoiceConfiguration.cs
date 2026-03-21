using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.InvoiceModels;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> b)
    {
        b.ToTable("invoices");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.InvoiceNumber)
            .HasColumnName("invoice_number")
            .IsRequired()
            .HasMaxLength(60);

        b.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        b.Property(x => x.ProjectId).HasColumnName("project_id");
        b.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();

        b.Property(x => x.IssueDate).HasColumnName("issue_date").HasColumnType("date").IsRequired();
        b.Property(x => x.DueDate).HasColumnName("due_date").HasColumnType("date").IsRequired();

        b.Property(x => x.Currency).HasColumnName("currency").IsRequired().HasMaxLength(10);

        b.Property(x => x.SubtotalAmount).HasColumnName("subtotal_amount").HasPrecision(18, 2).IsRequired();
        b.Property(x => x.TaxAmount).HasColumnName("tax_amount").HasPrecision(18, 2).IsRequired();
        b.Property(x => x.TotalAmount).HasColumnName("total_amount").HasPrecision(18, 2).IsRequired();

        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);

        b.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id").IsRequired();

        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        b.Property(x => x.CustomerNameSnapshot).HasColumnName("customer_name_snapshot").HasMaxLength(300);
        b.Property(x => x.CustomerTaxIdSnapshot).HasColumnName("customer_tax_id_snapshot").HasMaxLength(50);
        b.Property(x => x.BillingAddressSnapshot).HasColumnName("billing_address_snapshot").HasMaxLength(500);

        // relacje (dwukierunkowe)
        b.HasOne(x => x.Project)
            .WithMany(p => p.Invoices)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.CreatedByUser)
            .WithMany(u => u.InvoicesCreated)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.InvoiceNumber).IsUnique();
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.CustomerId);
        b.HasIndex(x => x.ProjectId);
        b.HasIndex(x => x.IssueDate);
        b.HasIndex(x => x.CreatedByUserId);
        b.HasIndex(x => new { x.CustomerId, x.IssueDate });

        // constrainty
        b.HasCheckConstraint("ck_invoice_due_after_issue", "due_date >= issue_date");
        b.HasCheckConstraint("ck_invoice_amounts_nonneg", "subtotal_amount >= 0 AND tax_amount >= 0 AND total_amount >= 0");
        b.HasCheckConstraint("ck_invoice_total_match", "total_amount = subtotal_amount + tax_amount");
        b.HasCheckConstraint("ck_invoice_currency_not_empty", "currency <> ''");
        b.HasCheckConstraint("ck_invoice_number_not_empty", "invoice_number <> ''");
    }
}
