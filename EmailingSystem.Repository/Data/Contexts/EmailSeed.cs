using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository.Data.Contexts
{
    public static class EmailSeed
    {
        public async static void SeedIdentity(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole("Admin"),
                    
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    Email = "Mo7amed6102003@gmail.com",
                    UserName = "Mohamed_10",
                    PhoneNumber = "0100 250 3550",
                    Name = "Mohamed Essam",

                };

                await userManager.CreateAsync(user, "12345678");

                var Admin = await userManager.FindByEmailAsync("Mo7amed6102003@gmail.com");

                if (Admin != null)
                    await userManager.AddToRoleAsync(Admin, "Admin");
            }

        }
    }
}
