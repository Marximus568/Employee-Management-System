using Domain.Models;

namespace Domain.Entities;

public class Employee : Person
{
    public string Position { get; set; }
    public decimal Salary { get; set; }

    public DateOnly HireDate { get; set; }

    public string Status { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<EmployeeEducation> EducationRecords { get; set; }
    
    public string ProfessionalProfile { get; set; }
}