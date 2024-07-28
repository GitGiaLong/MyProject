using System.Runtime.Serialization;

namespace Entities.Catelogies.TheWorld
{
    public enum ETheWorld
    {
        /// <summary>
        /// Country
        /// </summary>
        [EnumMember(Value = "Country")] Country,

        /// <summary>
        /// Provice
        /// </summary>
        [EnumMember(Value = "Provoice")] Provoice,

        /// <summary>
        /// Distrist
        /// </summary>
        [EnumMember(Value = "Distrist")] Distrist,

        /// <summary>
        /// Town
        /// </summary>
        [EnumMember(Value = "Town")] Town
    }
}
