using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProCredit.EmployeeManagement.Application.Abstractions;
using ProCredit.EmployeeManagement.Application.DTOs;

namespace ProCredit.EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/cargos")]
[Authorize]
public class CargosController(ICargoRepository cargoRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CargoDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await cargoRepository.GetAllAsync(cancellationToken));
}
