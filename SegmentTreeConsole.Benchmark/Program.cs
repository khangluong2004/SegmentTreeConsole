using BenchmarkDotNet.Running;
using SegmentTreeConsole.Benchmark;

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
