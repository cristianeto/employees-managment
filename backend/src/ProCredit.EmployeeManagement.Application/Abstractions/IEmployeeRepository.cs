using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface IEmployeeRepository
{
    /// <summary>Backed by sp_GetEmployeesByArea; areaName null returns all employees.</summary>
    Task<IReadOnlyList<EmployeeDto>> GetAllAsync(string? areaName, CancellationToken cancellationToken);

    Task<bool> DocumentIdExistsAsync(string documentId, CancellationToken cancellationToken);

    Task AddAsync(Employee employee, CancellationToken cancellationToken);
}
