using CarService.Application.DTOs;
using CarService.Application.Features.Parts.Commands;
using CarService.Application.Features.Parts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PartDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PartDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPartsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PartDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPartByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PartDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PartDto>> Create([FromBody] CreatePartDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePartCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
