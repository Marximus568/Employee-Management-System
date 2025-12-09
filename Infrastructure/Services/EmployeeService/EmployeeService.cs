using Application.DTOs.Employee;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EmployeeService;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(ApplicationDbContext context, IMapper mapper, ILogger<EmployeeService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        _logger.LogInformation("Creating new employee with document {Document}", createEmployeeDto.Document);

        var employee = _mapper.Map<Employee>(createEmployeeDto);

        // Ensure Department exists validation could go here
        
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        // Reload to get populated fields like Department if needed, 
        // but for now we just return the mapped object. 
        // If we need Department Name eagerly loaded, we might need to query it back or load reference.
        await _context.Entry(employee).Reference(e => e.Department).LoadAsync();

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Department)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
            
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }
}
