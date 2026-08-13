namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface IPasswordHasher
{
    bool Verify(string password, string passwordHash);
}
