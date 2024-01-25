using System.Runtime.Serialization;

namespace Entities.Application.Connect.Api
{
    public enum EnumMethodApi
    {
        /// <summary>
        /// Get-Select data
        /// </summary>
        [EnumMember(Value = "GET")] A,

        /// <summary>
        /// Post-Insert data
        /// </summary>
        [EnumMember(Value = "POST")] B,

        /// <summary>
        /// Put-Update data
        /// </summary>
        [EnumMember(Value = "PUT")] C,

        /// <summary>
        /// Delete-Drop data
        /// </summary>
        [EnumMember(Value = "DELETE")] D
    }
}
