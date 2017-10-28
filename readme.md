# GraphemeSplitter

A C# implementation of the Unicode grapheme cluster breaking algorithm.

## NuGet package

https://www.nuget.org/packages/GraphemeSplitter/

```powershell
Install-Package GraphemeSplitter
```

## Sample

```cs
using GraphemeSplitter;
using static System.Console;
using static System.String;

public partial class Program
{
    static string Split(string s) => Join(", ", s.GetGraphemes());

    static void Main()
    {
        WriteLine(Split("👨‍👨‍👧‍👦👩‍👩‍👧‍👦👨‍👨‍👧‍👦")); // 👨‍👨‍👧‍👦, 👩‍👩‍👧‍👦, 👨‍👨‍👧‍👦
    }
}
```

[Web Sample](tree/master/RazorPageSample):


![Razor Page Sample](doc/RazorPageSample.png)

## Acknowledgements

This library is indluenced by
- https://github.com/devongovett/grapheme-breaker
- https://github.com/orling/grapheme-splitter
- https://github.com/unicode-rs/unicode-segmentation
