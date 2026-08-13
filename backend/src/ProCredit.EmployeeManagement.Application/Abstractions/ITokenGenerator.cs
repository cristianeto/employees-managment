using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
