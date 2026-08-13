namespace ProCredit.EmployeeManagement.Application.DTOs;

public record CargoDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
