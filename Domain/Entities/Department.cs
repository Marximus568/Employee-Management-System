using Domain.Models;

namespace Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; }

    // Reverse Navigation
    public ICollection<Employee> Employees { get; set; }
}