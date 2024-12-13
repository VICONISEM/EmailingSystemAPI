using EmailingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Services.Contracts
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(ApplicationUser User ,UserManager<ApplicationUser> userManager);
    }
}
