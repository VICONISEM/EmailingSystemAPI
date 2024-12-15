using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Enums
{
    public enum ConversationStatus
    {
        [EnumMember(Value ="Active")]
        Active,
        [EnumMember(Value = "Archived")]
        Archived,
        [EnumMember(Value = "Deleted")]
        Deleted,
        [EnumMember(Value = "Starred")]
        Starred,
        [EnumMember(Value = "Trash")]
        Trash
    }
}
