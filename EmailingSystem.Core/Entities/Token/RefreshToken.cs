﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities.Token
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => !IsExpired;

    }
}
