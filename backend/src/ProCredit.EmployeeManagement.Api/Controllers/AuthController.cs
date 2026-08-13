using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Application.Services;

namespace ProCredit.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController(AuthenticateUserService authenticateUserService, ITokenGenerator tokenGenerator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await authenticateUserService.ExecuteAsync(request.Username, request.Password, cancellationToken);
        var token = tokenGenerator.GenerateToken(user);
        return Ok(new LoginResponse { Token = token });
    }
}
