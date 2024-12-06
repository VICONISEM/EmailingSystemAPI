using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = null!;


        private string NormalizedNameAttribute = null!;

        public string NormalizedName
        {
            get { return NormalizedNameAttribute; }
            set { NormalizedNameAttribute = Name.Trim().ToUpper();}
        }

        public string NationalId { get; set; } = null!;
        public string? PicturePath { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int SignatureId { get; set; }
        public Signature Signature { get; set; } = null!;
    }
}
