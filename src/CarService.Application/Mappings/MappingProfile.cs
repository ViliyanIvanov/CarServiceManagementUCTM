using AutoMapper;
using CarService.Application.DTOs;
using CarService.Domain.Entities;

namespace CarService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<UpdateVehicleDto, Vehicle>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();

        CreateMap<Mechanic, MechanicDto>();
        CreateMap<CreateMechanicDto, Mechanic>();

        CreateMap<Part, PartDto>();
        CreateMap<CreatePartDto, Part>();

        CreateMap<ServiceOrder, ServiceOrderDto>()
            .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.ServiceOrderParts));
        CreateMap<ServiceOrderPart, ServiceOrderPartDto>();
    }
}
