using Microsoft.EntityFrameworkCore;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Infrastructure.Persistence;

namespace ProCredit.EmployeeManagement.Infrastructure.Repositories;

public class CargoRepository(AppDbContext context) : ICargoRepository
{
    public async Task<IReadOnlyList<CargoDto>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Cargos
            .OrderBy(c => c.Name)
            .Select(c => new CargoDto { Id = c.Id, Name = c.Name })
            .ToListAsync(cancellationToken);

    public Task<CargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => context.Cargos
            .Where(c => c.Id == id)
            .Select(c => new CargoDto { Id = c.Id, Name = c.Name })
            .FirstOrDefaultAsync(cancellationToken);
}
