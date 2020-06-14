### Running Benchmarks

**Requirements:**
* **.NET 3.1 SDK** or later.
* **[`R` project for Statistical Computing](https://www.r-project.org/) version 3.3.3 (2017-03-06)** or later.
  * Ensure `rscript.exe` is in your path as [instructed here](https://benchmarkdotnet.org/articles/configs/exporters.html#plots) so that plots can be generated.

You must compile this `Benchmark` project in `Release` mode.

```csharp
dotnet build -c Release
```

After compiling in `Release` mode, execute the following command from the command line:
```
$> dotnet benchmark bin\Release\netstandard2.0\Benchmark.dll
```

Pick your benchmark to run. Your benchmark results will be in `\BenchmarkDotNet.Artifacts`.

### Other Notes

**DO NOT** attempt to run these benchmarks through **Visual Studio**'s debugger or **F5** runner.

More on benchmarking:
http://benchmarkdotnet.org/index.htm
