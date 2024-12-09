using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Entities
{
    public class College : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Department> Departments { get; set; } = null!;

    }
}
