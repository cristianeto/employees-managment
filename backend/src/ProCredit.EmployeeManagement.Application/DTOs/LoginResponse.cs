namespace ProCredit.EmployeeManagement.Application.DTOs;

public record LoginResponse
{
    public required string Token { get; init; }
}
