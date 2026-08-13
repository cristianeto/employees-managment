namespace ProCredit.EmployeeManagement.Domain.Entities;

public class Cargo
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
