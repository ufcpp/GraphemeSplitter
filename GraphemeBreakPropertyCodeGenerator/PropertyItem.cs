namespace GraphemeSplitter
{
    struct PropertyItem
    {
        public int Min { get; }
        public int Max { get; }
        public GraphemeBreakProperty Property { get; }
        public PropertyItem(int min, int max, GraphemeBreakProperty property) => (Min, Max, Property) = (min, max, property);
        public override string ToString() => (Min, Max, Property).ToString();
    }
}
