using System;
using System.Text;
using Xunit;
using GraphemeSplitter;
using System.Linq;

namespace GraphemeSplitterTest
{
    public class StringCodePoint
    {
        [Fact]
        public void GetCodePoint()
        {
            var s = "aáαℵАあ亜🐭👩𩸽";
            var expected = new[] { "a", "á", "α", "ℵ", "А", "あ", "亜", "🐭", "👩", "𩸽" };

            Span<byte> utf32Bytes = Encoding.UTF32.GetBytes(s);
            var expectedCodePoints = utf32Bytes.NonPortableCast<byte, uint>();

            var actual = s.GetCodePoints().ToArray();

            Assert.Equal(expected.Length, actual.Length);
            Assert.Equal(expectedCodePoints.Length, actual.Length);

            for (int i = 0; i < expectedCodePoints.Length; i++)
            {
                Assert.Equal(expected[i], new StringSegment(s, actual[i].index, actual[i].count).ToString());
                Assert.Equal(expectedCodePoints[i], actual[i].codePoint);
            }
        }
    }
}
