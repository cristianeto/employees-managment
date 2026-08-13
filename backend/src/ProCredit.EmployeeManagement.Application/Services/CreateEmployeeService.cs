using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Application.Exceptions;
using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Application.Services;

public class CreateEmployeeService(
    IEmployeeRepository employeeRepository,
    IAreaRepository areaRepository,
    ICargoRepository cargoRepository)
{
    public async Task<EmployeeDto> ExecuteAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var area = await areaRepository.GetByIdAsync(request.AreaId, cancellationToken)
            ?? throw new NotFoundException($"Area with id {request.AreaId} was not found.");

        var cargo = await cargoRepository.GetByIdAsync(request.CargoId, cancellationToken)
            ?? throw new NotFoundException($"Cargo with id {request.CargoId} was not found.");

        if (await employeeRepository.DocumentIdExistsAsync(request.DocumentId, cancellationToken))
        {
            throw new ConflictException($"An employee with document id '{request.DocumentId}' already exists.");
        }

        var employee = new Employee
        {
            DocumentId = request.DocumentId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            MonthlySalary = request.MonthlySalary,
            AreaId = request.AreaId,
            CargoId = request.CargoId,
        };

        await employeeRepository.AddAsync(employee, cancellationToken);

        return new EmployeeDto
        {
            Id = employee.Id,
            DocumentId = employee.DocumentId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Age = employee.Age,
            MonthlySalary = employee.MonthlySalary,
            AreaId = area.Id,
            AreaName = area.Name,
            CargoId = cargo.Id,
            CargoName = cargo.Name,
        };
    }
}
