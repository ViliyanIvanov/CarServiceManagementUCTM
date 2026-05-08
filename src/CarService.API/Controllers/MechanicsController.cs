using CarService.Application.DTOs;
using CarService.Application.Features.Mechanics.Commands;
using CarService.Application.Features.Mechanics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MechanicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MechanicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MechanicDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MechanicDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMechanicsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MechanicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MechanicDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMechanicByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MechanicDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MechanicDto>> Create([FromBody] CreateMechanicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateMechanicCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
