namespace ProCredit.EmployeeManagement.Infrastructure.Persistence;

/// <summary>Keyless shape of the sp_GetEmployeesByArea result set. Query-only, never persisted.</summary>
public class EmployeeQueryResult
{
    public int Id { get; set; }
    public required string DocumentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public decimal MonthlySalary { get; set; }
    public int AreaId { get; set; }
    public required string AreaName { get; set; }
    public int CargoId { get; set; }
    public required string CargoName { get; set; }
}
