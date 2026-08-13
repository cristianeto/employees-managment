using Microsoft.EntityFrameworkCore;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Domain.Entities;
using ProCredit.EmployeeManagement.Infrastructure.Persistence;

namespace ProCredit.EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(string? areaName, CancellationToken cancellationToken)
    {
        var results = await context.EmployeeQueryResults
            .FromSqlInterpolated($"EXEC dbo.sp_GetEmployeesByArea @AreaName = {areaName}")
            .ToListAsync(cancellationToken);

        return results.Select(r => new EmployeeDto
        {
            Id = r.Id,
            DocumentId = r.DocumentId,
            FirstName = r.FirstName,
            LastName = r.LastName,
            Age = r.Age,
            MonthlySalary = r.MonthlySalary,
            AreaId = r.AreaId,
            AreaName = r.AreaName,
            CargoId = r.CargoId,
            CargoName = r.CargoName,
        }).ToList();
    }

    public Task<bool> DocumentIdExistsAsync(string documentId, CancellationToken cancellationToken)
        => context.Employees.AnyAsync(e => e.DocumentId == documentId, cancellationToken);

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync(cancellationToken);
    }
}
