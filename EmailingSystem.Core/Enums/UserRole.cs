using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Enums
{
    public enum UserRole
    {
        NormalUser,
        Secretary,
        ViceDeanForEnvironment,
        ViceDeanForStudentsAffairs,
        ViceDeanForPostgraduatStudies,

        Dean,
        VicePresedientForEnvironment,
        VicePresedientForStudentsAffairs,
        VicePresedientForPostgraduatStudies,

        Presedient,
        CollegeAdmin,
        Admin
    }
}
