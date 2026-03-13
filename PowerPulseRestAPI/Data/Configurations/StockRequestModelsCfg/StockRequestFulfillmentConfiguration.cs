using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerPulseRestAPI.Data.Models.StockRequestModels;

public class StockRequestFulfillmentConfiguration : IEntityTypeConfiguration<StockRequestFulfillment>
{
    public void Configure(EntityTypeBuilder<StockRequestFulfillment> b)
    {
        b.ToTable("stock_request_fulfillments");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.StockRequestId).HasColumnName("stock_request_id").IsRequired();
        b.Property(x => x.IssuedByUserId).HasColumnName("issued_by_user_id").IsRequired();
        b.Property(x => x.IssuedAt).HasColumnName("issued_at").IsRequired();
        b.Property(x => x.Note).HasColumnName("note").HasMaxLength(2000);
        b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        b.HasOne(x => x.StockRequest)
            .WithMany(sr => sr.Fulfillments)
            .HasForeignKey(x => x.StockRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.IssuedByUser)
            .WithMany(u => u.StockRequestFulfillmentsIssued)
            .HasForeignKey(x => x.IssuedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.StockRequestId);
        b.HasIndex(x => x.IssuedAt);
        b.HasIndex(x => x.IssuedByUserId);
    }
}
