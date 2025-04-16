using Libraries.Custom.Enums.Icon;
using Libraries.Custom.Icons.Font.Icons;
using Libraries.Custom.Interfaces.Icon;

namespace Libraries.Custom.Icons
{
    public class Icon : IconBase<IconBlock, IconType>, IIconFont
    {
        public Icon(IconType icon) : base(icon) { }

        public IconFont IconFont
        {
            get { return IconBlock.IconFont; }
            set { IconBlock.IconFont = value; }
        }
    }
}
