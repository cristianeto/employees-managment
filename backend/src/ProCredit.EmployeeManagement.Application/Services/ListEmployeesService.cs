using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;

namespace ProCredit.EmployeeManagement.Application.Services;

public class ListEmployeesService(IEmployeeRepository employeeRepository)
{
    public Task<IReadOnlyList<EmployeeDto>> ExecuteAsync(string? areaName, CancellationToken cancellationToken)
        => employeeRepository.GetAllAsync(areaName, cancellationToken);
}
