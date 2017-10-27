using GraphemeSplitter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Globalization.NumberStyles;

namespace GraphemeBreakPropertyCodeGenerator
{
    class Loader
    {
        public static PropertyItem[] LoadProperties(UnicodeVersion version)
            => (
            from line in GetUcdDefinition(version).Result
            let commentRemoved = line.Split('#').FirstOrDefault()
            where !string.IsNullOrWhiteSpace(commentRemoved)
            let x = commentRemoved.Split(';')
            let range = x[0]
            let nums = range.Split('.')
            let min = nums[0]
            let max = nums.Length == 3 ? nums[2] : nums[0]
            let property = x[1]
            select new PropertyItem(int.Parse(min, HexNumber), int.Parse(max, HexNumber), Enum.Parse<GraphemeBreakProperty>(property))
            ).ToArray();

        static Dictionary<UnicodeVersion, string> Urls = new Dictionary<UnicodeVersion, string>
        {
            { UnicodeVersion.V9, "http://www.unicode.org/Public/9.0.0/ucd/auxiliary/GraphemeBreakProperty.txt" },
            { UnicodeVersion.V10, "http://www.unicode.org/Public/10.0.0/ucd/auxiliary/GraphemeBreakProperty.txt" },
        };

        static async Task<string[]> GetUcdDefinition(UnicodeVersion version)
        {
            var path = version + ".txt";
            if (File.Exists(path)) return await File.ReadAllLinesAsync(path);

            var url = Urls.TryGetValue(version, out var x) ? x : throw new IndexOutOfRangeException();

            var c = new HttpClient();
            var res = await c.GetAsync(url);
            var text = await res.Content.ReadAsStringAsync();

            await File.WriteAllTextAsync(path, text);

            return text.Split('\n');
        }
    }
}
