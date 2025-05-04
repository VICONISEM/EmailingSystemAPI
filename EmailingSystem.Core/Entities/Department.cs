using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class Department : BaseEntity
    {

        public string Name { get; set; } =  null! ;
        public string Abbreviation { get; set; } = null!;
        public int CollegeId { get; set; }
        public virtual College College { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
