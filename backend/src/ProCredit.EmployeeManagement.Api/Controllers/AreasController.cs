using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;

namespace ProCredit.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/areas")]
[Authorize]
public class AreasController(IAreaRepository areaRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AreaDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await areaRepository.GetAllAsync(cancellationToken));
}
