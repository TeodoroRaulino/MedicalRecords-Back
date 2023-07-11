using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public static class DataSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Name = "Teodoro Raulino",
                        Password = passwordHash,
                        Email = "teodoro@example.com",
                        Role = Enums.UserRole.Doctor,
                        Id = 1
                    }
            );
        }
    }
}
