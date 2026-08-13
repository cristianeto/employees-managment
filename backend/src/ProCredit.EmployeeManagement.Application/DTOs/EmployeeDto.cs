namespace ProCredit.EmployeeManagement.Application.DTOs;

public record EmployeeDto
{
    public required int Id { get; init; }
    public required string DocumentId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int Age { get; init; }
    public required decimal MonthlySalary { get; init; }
    public required int AreaId { get; init; }
    public required string AreaName { get; init; }
    public required int CargoId { get; init; }
    public required string CargoName { get; init; }
}
