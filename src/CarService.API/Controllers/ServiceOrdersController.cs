using CarService.Application.DTOs;
using CarService.Application.Features.ServiceOrders.Commands;
using CarService.Application.Features.ServiceOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceOrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetServiceOrderByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceOrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceOrderDto>> Create([FromBody] CreateServiceOrderDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateServiceOrderCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
