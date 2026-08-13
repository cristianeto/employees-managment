using ProCredit.EmployeeManagement.Application.DTOs;

namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface IAreaRepository
{
    Task<IReadOnlyList<AreaDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<AreaDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
