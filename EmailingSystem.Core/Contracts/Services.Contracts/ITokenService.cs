﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Services.Contracts
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync();
    }
}