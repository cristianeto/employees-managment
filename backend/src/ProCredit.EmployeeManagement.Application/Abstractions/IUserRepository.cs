using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}
