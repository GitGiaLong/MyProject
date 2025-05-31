namespace Libraries.Custom.Structs.Virtualizings
{

    /// <summary>
    /// Items range.
    /// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
    /// </summary>
    public readonly struct ItemRange
    {
        public int StartIndex { get; }

        public int EndIndex { get; }

        public ItemRange(int startIndex, int endIndex) : this()
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public readonly bool Contains(int itemIndex) => itemIndex >= StartIndex && itemIndex <= EndIndex;
    }
}
