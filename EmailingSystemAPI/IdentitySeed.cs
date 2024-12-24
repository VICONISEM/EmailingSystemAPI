using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace HospitalML
{
    public static class IdentitySeed
    {
        public async static void SeedIdentity(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole<int>>
                {
                    new IdentityRole<int>("NormalUser"),
                    new IdentityRole<int>("Secretary"),

                    new IdentityRole<int>("ViceDeanForEnvironment"),
                    new IdentityRole<int>("ViceDeanForStudentsAffairs"),
                    new IdentityRole<int>("ViceDeanForPostgraduatStudies"),

                    new IdentityRole<int>("Dean"),

                    new IdentityRole<int>("VicePresedientForEnvironment"),
                    new IdentityRole<int>("VicePresedientForStudentsAffairs"),
                    new IdentityRole<int>("VicePresedientForPostgraduatStudies"),

                    new IdentityRole<int>("Presedient"),

                    new IdentityRole<int>("CollegeAdmin"),
                    new IdentityRole<int>("Admin")
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {

                var users = new List<ApplicationUser>()
                {
                    
                    new ApplicationUser()
                    {
                        Email = "victornisemAdmin@gmail.com",
                        UserName = "VictorNisem",
                        PhoneNumber = "0100 250 3551",
                        NationalId = "30310200200955",
                        Name = "Victor Nisem",
                        NormalizedName = "VICTOR NISEM"
                    },
                    
                    new ApplicationUser()
                    {
                        Email = "AmmarAdmin@gmail.com",
                        UserName = "AmmarYasser",
                        PhoneNumber = "0102 612 8908",
                        NationalId = "30212193100157",
                        Name = "Ammar Yasser",
                        NormalizedName = "AMMAR YASSER"

                    },

                };


            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "12345678");
                await userManager.AddToRoleAsync(user, "Admin");


            }
            }

        }
    }
}
