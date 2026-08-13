using Microsoft.EntityFrameworkCore;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Domain.Entities;
using ProCredit.EmployeeManagement.Infrastructure.Persistence;

namespace ProCredit.EmployeeManagement.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
}
