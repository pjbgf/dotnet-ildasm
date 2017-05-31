``` ini

BenchmarkDotNet=v0.10.6, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-4710HQ CPU 2.50GHz (Haswell), ProcessorCount=8
Frequency=2435769 Hz, Resolution=410.5480 ns, Timer=TSC
  [Host] : Clr 4.0.30319.42000, 64bit LegacyJIT-v4.7.2046.0DEBUG
  Clr    : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2046.0
  Core   : .NET Core 4.6.25211.01, 64bit RyuJIT


```
 |                    Method |  Job | Runtime |     Mean |     Error |    StdDev |   Median |
 |-------------------------- |----- |-------- |---------:|----------:|----------:|---------:|
 |  FileBufferedOutputWriter |  Clr |     Clr | 629.2 us | 13.249 us | 36.269 us | 617.5 us |
 | FileBufferedOutputWriter2 |  Clr |     Clr | 612.0 us | 11.210 us |  9.361 us | 610.7 us |
 |          FileOutputWriter |  Clr |     Clr | 728.5 us | 16.326 us | 18.146 us | 726.6 us |
 |  FileBufferedOutputWriter | Core |    Core | 400.0 us |  7.725 us |  7.587 us | 398.9 us |
 | FileBufferedOutputWriter2 | Core |    Core | 403.2 us |  9.627 us |  9.005 us | 404.5 us |
 |          FileOutputWriter | Core |    Core | 511.8 us | 10.235 us | 12.184 us | 507.4 us |
