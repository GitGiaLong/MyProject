namespace Libraries.Custom.Icon.Font.Icons
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
