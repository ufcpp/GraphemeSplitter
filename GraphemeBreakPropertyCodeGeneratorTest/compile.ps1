$csc = (Get-VSSetupInstance)[0].InstallationPath + '\MSBuild\15.0\Bin\Roslyn\csc.exe'

# LinearIf: linear-search if
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.if.cs /out:bin\LinearIf.dll /target:library 

# Switch: switch (case x:)
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switch.cs /out:bin\Switch.dll /target:library 

# SwitchWhen: switch (case uint x when min <= x && x <= max)
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.switchwhen.cs /out:bin\SwitchWhen.dll /target:library 

# BinarySearchIf: binary-search if
& $csc ..\GraphemeBreakPropertyCodeGenerator\GraphemeBreakProperty.cs .\Benchmark.binif.cs /out:bin\BinarySearchIf.dll /target:library 
