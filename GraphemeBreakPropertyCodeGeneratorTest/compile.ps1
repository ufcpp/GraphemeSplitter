$csc = (Get-VSSetupInstance)[0].InstallationPath + '\MSBuild\15.0\Bin\Roslyn\csc.exe'

# LinearIf: linear-search if
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.if.cs /out:bin\Debug\LinearIf.dll /target:library /o-
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.if.cs /out:bin\Release\LinearIf.dll /target:library /o+

# Switch: switch (case x:)
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switch.cs /out:bin\Debug\Switch.dll /target:library /o-
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switch.cs /out:bin\Release\Switch.dll /target:library /o+

# SwitchWhen: switch (case uint x when min <= x && x <= max)
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switchwhen.cs /out:bin\Debug\SwitchWhen.dll /target:library /o-
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switchwhen.cs /out:bin\Release\SwitchWhen.dll /target:library /o+

# BinarySearchIf: binary-search if
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.binif.cs /out:bin\Debug\BinarySearchIf.dll /target:library /o-
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.binif.cs /out:bin\Release\BinarySearchIf.dll /target:library /o+
