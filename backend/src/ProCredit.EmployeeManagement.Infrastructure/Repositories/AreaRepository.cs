using Microsoft.EntityFrameworkCore;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Infrastructure.Persistence;

namespace ProCredit.EmployeeManagement.Infrastructure.Repositories;

public class AreaRepository(AppDbContext context) : IAreaRepository
{
    public async Task<IReadOnlyList<AreaDto>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Areas
            .OrderBy(a => a.Name)
            .Select(a => new AreaDto { Id = a.Id, Name = a.Name })
            .ToListAsync(cancellationToken);

    public Task<AreaDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => context.Areas
            .Where(a => a.Id == id)
            .Select(a => new AreaDto { Id = a.Id, Name = a.Name })
            .FirstOrDefaultAsync(cancellationToken);
}
