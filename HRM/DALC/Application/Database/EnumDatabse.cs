using System.Runtime.Serialization;

namespace DALC.Application.Database
{
    public enum EnumDatabse
    {
        /// <summary>
        /// HRM
        /// </summary>
        [EnumMember(Value = "HRM")] HRM,

        /// <summary>
        /// TheWorld
        /// </summary>
        [EnumMember(Value = "TheWorld")] TheWorld,
    }
}
