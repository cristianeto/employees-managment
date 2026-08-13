using ProCredit.EmployeeManagement.Application.DTOs;

namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface ICargoRepository
{
    Task<IReadOnlyList<CargoDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
