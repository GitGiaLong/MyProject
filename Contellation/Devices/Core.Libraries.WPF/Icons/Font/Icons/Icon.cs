using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Font.Icons;
using Core.Libraries.WPF.Icons.Interfaces;

namespace Core.Libraries.WPF.Icons
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
