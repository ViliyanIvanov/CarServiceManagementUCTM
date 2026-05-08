using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Features.Vehicles.Commands;
using CarService.Application.Features.Vehicles.Queries;
using CarService.Application.Interfaces;
using CarService.Application.Mappings;
using CarService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CarService.Application.Tests.Vehicles;

public class VehicleHandlersTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IRepository<Vehicle>> _repository = new();
    private readonly IMapper _mapper;

    public VehicleHandlersTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();

        _unitOfWork.SetupGet(u => u.Vehicles).Returns(_repository.Object);
    }

    private static Vehicle Sample(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        CustomerId = Guid.NewGuid(),
        Make = "Toyota",
        Model = "Corolla",
        Year = 2020,
        LicensePlate = "AB1234CD",
        Vin = "1HGCM82633A123456",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetVehiclesQueryHandler_ReturnsVehicles()
    {
        var vehicles = new List<Vehicle> { Sample(), Sample() };
        _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(vehicles);

        var handler = new GetVehiclesQueryHandler(_unitOfWork.Object, _mapper);
        var result = await handler.Handle(new GetVehiclesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().AllBeOfType<VehicleDto>();
    }

    [Fact]
    public async Task GetVehicleByIdQueryHandler_ReturnsVehicleById()
    {
        var entity = Sample();
        _repository.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        var handler = new GetVehicleByIdQueryHandler(_unitOfWork.Object, _mapper);
        var result = await handler.Handle(new GetVehicleByIdQuery(entity.Id), CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Make.Should().Be(entity.Make);
    }

    [Fact]
    public async Task GetVehicleByIdQueryHandler_Throws_WhenNotFound()
    {
        _repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?)null);

        var handler = new GetVehicleByIdQueryHandler(_unitOfWork.Object, _mapper);
        Func<Task> act = () => handler.Handle(new GetVehicleByIdQuery(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateVehicleCommandHandler_CreatesVehicle()
    {
        var dto = new CreateVehicleDto
        {
            CustomerId = Guid.NewGuid(),
            Make = "Honda",
            Model = "Civic",
            Year = 2022,
            LicensePlate = "CD5678EF",
            Vin = "2HGCM82633A654321"
        };

        Vehicle? captured = null;
        _repository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
            .Callback<Vehicle, CancellationToken>((v, _) => captured = v)
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateVehicleCommandHandler(_unitOfWork.Object, _mapper);
        var result = await handler.Handle(new CreateVehicleCommand(dto), CancellationToken.None);

        result.Should().NotBeNull();
        result.Make.Should().Be(dto.Make);
        captured.Should().NotBeNull();
        captured!.Id.Should().NotBe(Guid.Empty);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVehicleCommandHandler_UpdatesVehicle()
    {
        var entity = Sample();
        _repository.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new UpdateVehicleDto
        {
            Make = "Ford",
            Model = "Focus",
            Year = 2023,
            LicensePlate = "ZZ9999ZZ",
            Vin = "3HGCM82633A111111"
        };

        var handler = new UpdateVehicleCommandHandler(_unitOfWork.Object, _mapper);
        var result = await handler.Handle(new UpdateVehicleCommand(entity.Id, dto), CancellationToken.None);

        result.Should().BeTrue();
        entity.Make.Should().Be("Ford");
        entity.Model.Should().Be("Focus");
        _repository.Verify(r => r.Update(entity), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteVehicleCommandHandler_DeletesVehicle()
    {
        var entity = Sample();
        _repository.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new DeleteVehicleCommandHandler(_unitOfWork.Object);
        var result = await handler.Handle(new DeleteVehicleCommand(entity.Id), CancellationToken.None);

        result.Should().BeTrue();
        _repository.Verify(r => r.Remove(entity), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
