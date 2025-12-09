namespace Application.DTOs.Employee;

public class EmployeeEducationDto
{
    public int Id { get; set; }
    public string Institution { get; set; }
    public string Degree { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string City { get; set; }
}
