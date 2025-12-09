namespace Application.DTOs.Employee;

public class EmployeeDto
{
    public int Id { get; set; }
    
    // Person fields
    public string Document { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    // Employee fields
    public string Position { get; set; }
    public decimal Salary { get; set; }
    public DateOnly HireDate { get; set; }
    public string Status { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    
    public string ProfessionalProfile { get; set; }
    public List<EmployeeEducationDto> EducationRecords { get; set; } = new();
}
