using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProCredit.EmployeeManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGetEmployeesByAreaProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.sp_GetEmployeesByArea
                    @AreaName NVARCHAR(100) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        e.Id,
                        e.DocumentId,
                        e.FirstName,
                        e.LastName,
                        e.Age,
                        e.MonthlySalary,
                        a.Id   AS AreaId,
                        a.Name AS AreaName,
                        c.Id   AS CargoId,
                        c.Name AS CargoName
                    FROM dbo.Employees e
                    INNER JOIN dbo.Areas a ON a.Id = e.AreaId
                    INNER JOIN dbo.Cargos c ON c.Id = e.CargoId
                    WHERE @AreaName IS NULL OR a.Name = @AreaName
                    ORDER BY e.LastName, e.FirstName;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_GetEmployeesByArea;");
        }
    }
}
