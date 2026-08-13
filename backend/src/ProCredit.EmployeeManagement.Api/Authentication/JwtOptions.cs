namespace ProCredit.EmployeeManagement.Api.Authentication;

public class JwtOptions
{
    public required string SigningKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpiryMinutes { get; init; } = 60;
}
