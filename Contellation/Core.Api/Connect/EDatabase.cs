using System.Runtime.Serialization;

namespace Core.Api.Connect
{
    public enum EDatabase
    {
        /// <summary>
        /// HRM
        /// </summary>
        [EnumMember(Value = "HRM")] HRM,

        /// <summary>
        /// The World
        /// </summary>
        [EnumMember(Value = "TheWorld")] TheWorld,

        /// <summary>
        /// User Management
        /// </summary>
        [EnumMember(Value = "UserManagement")] UserManagement,
    }
}
