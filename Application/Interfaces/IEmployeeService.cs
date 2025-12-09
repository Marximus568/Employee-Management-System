using Application.DTOs.Employee;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
}
