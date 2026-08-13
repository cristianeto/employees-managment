using ProCredit.EmployeeManagement.Application.Abstractions;

namespace ProCredit.EmployeeManagement.Infrastructure.Security;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    public bool Verify(string password, string passwordHash) => PasswordHasher.Verify(password, passwordHash);
}
