using System;
using System.Text;
using Xunit;
using GraphemeSplitter;
using System.Linq;

namespace GraphemeSplitterTest
{
    public class TestStringExtensions
    {
        [Fact]
        public void GetCodePoints()
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

        public void aaa()
        {
            var s = "aáαℵАあ亜🐭👩𩸽Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍A̴̵̜̰͔ͫ͗͢L̠ͨͧͩ͘G̴̻͈͍͔̹̑͗̎̅͛́Ǫ̵̹̻̝̳͂̌̌͘!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞👩🏻‍👩🏻‍👦🏻‍👦🏻👩🏻‍👩🏻‍👦🏻‍👦🏼👩🏻‍👩🏻‍👦🏻‍👦🏽👩🏻‍👩🏻‍👦🏻‍👦🏾";
            var expected = new[] { "a", "á", "α", "ℵ", "А", "あ", "亜", "🐭", "👩", "𩸽", "Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍", "A̴̵̜̰͔ͫ͗͢", "L̠ͨͧͩ͘", "G̴̻͈͍͔̹̑͗̎̅͛́", "Ǫ̵̹̻̝̳͂̌̌͘", "!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞", "👩🏻‍👩🏻‍👦🏻‍👦🏻", "👩🏻‍👩🏻‍👦🏻‍👦🏼", "👩🏻‍👩🏻‍👦🏻‍👦🏽", "👩🏻‍👩🏻‍👦🏻‍👦🏾" };
            GetGraphemes(s, expected);
        }

        [Fact]
        public void GetGraphemesSingleUtf16() => GetGraphemes("aáαℵАあ亜", "a", "á", "α", "ℵ", "А", "あ", "亜");

        [Fact]
        public void GetGraphemesSingleCodePoint() => GetGraphemes("🐭👩𩸽👪", "🐭", "👩", "𩸽", "👪");

        [Fact]
        public void GetGraphemesCombining() => GetGraphemes("Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍A̴̵̜̰͔ͫ͗͢L̠ͨͧͩ͘G̴̻͈͍͔̹̑͗̎̅͛́Ǫ̵̹̻̝̳͂̌̌͘!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞", "Z͑ͫ̓ͪ̂ͫ̽͏̴̙̤̞͉͚̯̞̠͍", "A̴̵̜̰͔ͫ͗͢", "L̠ͨͧͩ͘", "G̴̻͈͍͔̹̑͗̎̅͛́", "Ǫ̵̹̻̝̳͂̌̌͘", "!͖̬̰̙̗̿̋ͥͥ̂ͣ̐́́͜͞");

        //[Fact]
        //public void GetGraphemesHangul() => GetGraphemes("", "");

        [Fact]
        public void GetGraphemesEmojiSkinTone() => GetGraphemes("👩🏻👩🏼👩🏽👩🏾👩🏿👨🏻👨🏼👨🏽👨🏾👨🏿", "👩🏻", "👩🏼", "👩🏽", "👩🏾", "👩🏿", "👨🏻", "👨🏼", "👨🏽", "👨🏾", "👨🏿");

        [Fact]
        public void GetGraphemesZwjEmoji() => GetGraphemes("👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩👩‍👩‍👧‍👧👩‍👩‍👧👩‍👧", "👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩‍👩", "👩‍👩‍👧‍👧", "👩‍👩‍👧", "👩‍👧");

        private static void GetGraphemes(string s, params string[] expected)
        {
            foreach (var x in expected)
            {
                var y = x.GetGraphemes().ToArray();
                Assert.Equal(1, y.Length);
            }

            var actual = s.GetGraphemes().ToArray();

            Assert.Equal(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i].ToString());
            }
        }
    }
}
