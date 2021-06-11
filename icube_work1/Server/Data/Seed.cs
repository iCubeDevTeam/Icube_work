using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class Seed
    {
        public static async Task SeedFactory(DataContext context)
        {
            if (await context.Factorys.AnyAsync()) return;
            
            var factoryData = await System.IO.File.ReadAllTextAsync("Data/FactorySeedData.json");
            var factorys = JsonSerializer.Deserialize<List<Factory>>(factoryData);
            foreach (var factory in factorys)
            {
                factory.Factoryname = factory.Factoryname.ToLower();
                context.Factorys.Add(factory);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedRole(DataContext context)
        {
            if (await context.Roles.AnyAsync()) return;
            
            var roleData = await System.IO.File.ReadAllTextAsync("Data/RoleSeedData.json");
            var roles = JsonSerializer.Deserialize<List<Role>>(roleData);
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;
            
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<User>>(userData);
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.Username = user.Username.ToLower();
                user.Passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                user.Passwordsalt = hmac.Key;
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}