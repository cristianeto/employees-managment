using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.Exceptions;
using ProCredit.EmployeeManagement.Domain.Entities;

namespace ProCredit.EmployeeManagement.Application.Services;

public class AuthenticateUserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
{
    public async Task<User> ExecuteAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user is null || !passwordHasher.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        return user;
    }
}
