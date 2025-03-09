using Libraries.Custom.Icons.Enums;
using Libraries.Custom.Icons.Font.Icons;
using Libraries.Custom.Icons.Interfaces;

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
