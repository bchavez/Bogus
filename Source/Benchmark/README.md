### Running Benchmarks

When running benchmarks, please make sure you compile this project in `Release` mode.

Once compiled go to `\bin\Release\` and execute from the command line:
```
$> Benchmark.exe
```

Your benchmark results will be in `\bin\Release\BenchmarkDotNet.Artifacts`.

**DO NOT** attempt to run these benchmarks through **Visual Studio**'s debugger or **F5** runner.

Also, make sure you've installed [**`R`**](https://www.r-project.org/) and that `rscript.exe` is in your path as [instructed here](http://benchmarkdotnet.org/Configs/Exporters.htm#plots) so that plots can be generated.

More on benchmarking:
http://benchmarkdotnet.org/index.htm
