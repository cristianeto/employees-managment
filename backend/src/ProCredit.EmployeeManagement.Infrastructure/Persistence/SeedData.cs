using Microsoft.EntityFrameworkCore;
using ProCredit.EmployeeManagement.Domain.Entities;
using ProCredit.EmployeeManagement.Infrastructure.Security;

namespace ProCredit.EmployeeManagement.Infrastructure.Persistence;

public static class SeedData
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>().HasData(
            new Area { Id = 1, Name = "Recursos Humanos" },
            new Area { Id = 2, Name = "Finanzas" },
            new Area { Id = 3, Name = "Contabilidad" },
            new Area { Id = 4, Name = "Marketing" },
            new Area { Id = 5, Name = "Sistemas" },
            new Area { Id = 6, Name = "Banca Empresas" },
            new Area { Id = 7, Name = "Banca Personas" }
        );

        modelBuilder.Entity<Cargo>().HasData(
            new Cargo { Id = 1, Name = "Analista de Recursos Humanos" },
            new Cargo { Id = 2, Name = "Contador Senior" },
            new Cargo { Id = 3, Name = "Supervisor de Créditos" },
            new Cargo { Id = 4, Name = "Diseñador UX/UI" },
            new Cargo { Id = 5, Name = "Especialista de Sistemas" }
        );

        // Test user: username "admin", password "Admin123!" (documented in README).
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "100000.ngQR7nuViQXnp62v7+AIeg==.amtMMxaT/iNYuBcbQrMzjqmWziY5Z4OuN6t98TnYPUY="
            }
        );
    }
}
