using System.ComponentModel.DataAnnotations;

namespace ProCredit.EmployeeManagement.Application.DTOs;

public record CreateEmployeeRequest
{
    [Required, MaxLength(20)]
    public required string DocumentId { get; init; }

    [Required, MaxLength(100)]
    public required string FirstName { get; init; }

    [Required, MaxLength(100)]
    public required string LastName { get; init; }

    [Range(1, 120)]
    public required int Age { get; init; }

    [Range(0.01, double.MaxValue)]
    public required decimal MonthlySalary { get; init; }

    [Range(1, int.MaxValue)]
    public required int AreaId { get; init; }

    [Range(1, int.MaxValue)]
    public required int CargoId { get; init; }
}
