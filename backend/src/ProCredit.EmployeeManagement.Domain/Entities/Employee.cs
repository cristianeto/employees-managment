namespace ProCredit.EmployeeManagement.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string DocumentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public decimal MonthlySalary { get; set; }

    public int AreaId { get; set; }
    public Area? Area { get; set; }

    public int CargoId { get; set; }
    public Cargo? Cargo { get; set; }
}
