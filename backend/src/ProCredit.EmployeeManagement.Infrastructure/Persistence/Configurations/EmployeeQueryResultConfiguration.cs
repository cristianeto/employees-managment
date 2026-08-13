using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProCredit.EmployeeManagement.Infrastructure.Persistence.Configurations;

public class EmployeeQueryResultConfiguration : IEntityTypeConfiguration<EmployeeQueryResult>
{
    public void Configure(EntityTypeBuilder<EmployeeQueryResult> builder)
    {
        // Result-set shape for sp_GetEmployeesByArea only; not backed by a table or view.
        builder.HasNoKey();
        builder.ToView(null);
    }
}
