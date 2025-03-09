using System.Runtime.Serialization;

namespace Core.Entities.Systems
{
    public enum Orientations
    {
        [EnumMember(Value = "Normal")]
        Normal = 0,

        [EnumMember(Value = "Horizontal")]
        Horizontal,

        [EnumMember(Value = "Left")]
        Left,

        [EnumMember(Value = "Right")]
        Right,

        [EnumMember(Value = "Vertical")]
        Vertical,

        [EnumMember(Value = "Top")]
        Top,

        [EnumMember(Value = "Bottom")]
        Bottom,
    }
}
