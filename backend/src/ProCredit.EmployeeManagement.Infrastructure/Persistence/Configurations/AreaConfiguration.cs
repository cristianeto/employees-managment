using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Name).IsUnique();
    }
}
