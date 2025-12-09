using AutoMapper;
using Application.DTOs.Employee;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active")); // Default status

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
    }
}
