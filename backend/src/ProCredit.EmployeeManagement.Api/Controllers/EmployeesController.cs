using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProCredit.EmployeeManagement.Application.DTOs;
using ProCredit.EmployeeManagement.Application.Services;

namespace ProCredit.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController(
    ListEmployeesService listEmployeesService,
    CreateEmployeeService createEmployeeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetAll(
        [FromQuery] string? area, CancellationToken cancellationToken)
        => Ok(await listEmployeesService.ExecuteAsync(area, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(
        CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await createEmployeeService.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { }, employee);
    }
}
