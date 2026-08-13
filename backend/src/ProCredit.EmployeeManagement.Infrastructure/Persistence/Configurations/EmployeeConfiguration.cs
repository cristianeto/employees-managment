using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.DocumentId)
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(e => e.DocumentId).IsUnique();

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Age).IsRequired();
        builder.Property(e => e.MonthlySalary).HasColumnType("decimal(18,2)");

        builder.HasOne(e => e.Area)
            .WithMany(a => a.Employees)
            .HasForeignKey(e => e.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Cargo)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CargoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
