# SysCompat Performance

## ThrowHelpers

The throw helpers are based on existing code from .NET 8.0 repository, adapted
to be useful also for .NET Framework 4.0 (with language version C# 10).

Not all throw helpers are profiled, as many are similar. But presented here is a
profile between this repository and .NET Core, that it can be estimated the cost
of using this repository.

```text
Results = net48

BenchmarkDotNet=v0.13.12 OS=Windows 10 (10.0.19045.4046/22H2/2022Update)
Intel Core i7-6700T CPU 2.80GHz (Skylake), 1 CPU(s), 8 logical and 4 physical core(s)
  [HOST] : .NET Framework 4.8.1 (4.8.9181.0), X64 RyuJIT
```

```text
Results = net6

BenchmarkDotNet=v0.13.12 OS=Windows 10 (10.0.19045.4046/22H2/2022Update)
Intel Core i7-6700T CPU 2.80GHz (Skylake), 1 CPU(s), 8 logical and 4 physical core(s)
  [HOST] : .NET 6.0.27 (6.0.2724.6912), X64 RyuJIT
```

```text
Results = net8

BenchmarkDotNet=v0.13.12 OS=Windows 10 (10.0.19045.4046/22H2/2022Update)
Intel Core i7-6700T CPU 2.80GHz (Skylake), 1 CPU(s), 8 logical and 4 physical core(s)
  [HOST] : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT
```

| Project 'syscompat' Type | Method                         | mean (net48) | stderr | mean (net6) | stderr | mean (net8) | stderr |
|:-------------------------|:-------------------------------|-------------:|-------:|------------:|-------:|------------:|-------:|
| ThrowIfArray             | ThrowIfArrayOutOfBounds        | 3.10         | 0.01   | 3.39        | 0.01   | 3.40        | 0.00   |
| ThrowIfEnum              | ThrowIfEnumHasFlag             | 23.47        | 0.05   | 1.38        | 0.00   | 1.14        | 0.00   |
| ThrowIfEnum              | ThrowIfEnumUndefined           | 250.76       | 0.92   | 111.15      | 0.29   | 24.85       | 0.04   |
| ThrowIfNullBenchmark     | ThrowIfNull                    | 0.30         | 0.00   | 0.00        | 0.00   | 0.01        | 0.00   |
| ThrowIfNullBenchmark     | ThrowIfNull_System             | -            | -      | 0.00        | 0.00   | 0.00        | 0.00   |
| ThrowIfNullBenchmark     | ThrowIfNullOrWhiteSpace        | 4.15         | 0.01   | 3.69        | 0.01   | 1.65        | 0.01   |
| ThrowIfNullBenchmark     | ThrowIfNullOrWhiteSpace_System | -            | -      | -           | -      | 1.58        | 0.01   |
| ThrowIfNullBenchmark     | ThrowIfZero                    | 0.01         | 0.00   | 0.00        | 0.00   | 0.01        | 0.00   |
| ThrowIfNullBenchmark     | ThrowIfZero_System             | -            | -      | -           | -      | 0.01        | 0.00   |
