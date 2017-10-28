using System;

namespace GraphemeSplitter
{
    public struct StringSegment
    {
        public string String;
        public int Offset;
        public int Count;

        public StringSegment(string @string, int offset, int count) => (String, Offset, Count) = (@string, offset, count);
        public void Deconstruct(out string @string, out int offset, out int count) => (@string, offset, count) = (String, Offset, Count);
        public override string ToString() => String.Substring(Offset, Count);
    }
}
