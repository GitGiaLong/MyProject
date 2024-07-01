using System.Runtime.Serialization;

namespace Core.Entities.Applications.Connects.Enums
{
    public enum EDatabase
    {
        /// <summary>
        /// HRM
        /// </summary>
        [EnumMember(Value = "HRM")] HRM,

        /// <summary>
        /// TheWorld
        /// </summary>
        [EnumMember(Value = "TheWorld")] TheWorld,


        /// <summary>
        /// TheWorld
        /// </summary>
        [EnumMember(Value = "UserManagement")] UserManagement,
    }
}
