using System.Runtime;
using SegmentTreeConsole.Factory;
using System.Diagnostics;

// Sample data for the sum segment tree
int[][] data = [[2], [1], [3], [4], [5], [7], [8], [9]];

Console.WriteLine("=== SUM SEGMENT TREE DEMO (using existing generic API) ===");
Console.WriteLine($"Input array: {string.Join(", ", data.Select(elem => elem[0]))}");


var minMaxSumLazySegmentTreeFactory = new MinMaxSumLazySegmentTreeFactory();
var minMaxSumLazySegmentTree = minMaxSumLazySegmentTreeFactory.CreateSegmentTree(data);

Console.WriteLine($"{string.Join(", ", minMaxSumLazySegmentTree.QueryRange(0, 3)!)}");
Console.WriteLine($"{string.Join(", ", minMaxSumLazySegmentTree.QueryRange(2, 5)!)}");

minMaxSumLazySegmentTree.UpdateRange(2, 0, 3);
Console.WriteLine($"{string.Join(", ", minMaxSumLazySegmentTree.QueryRange(0, 3)!)}");

minMaxSumLazySegmentTree.UpdateRange(2, 3, 5);
Console.WriteLine($"{string.Join(", ", minMaxSumLazySegmentTree.QueryRange(0, 3)!)}");

// Perf test to compare with C++ :D
int SIZE = 100_000_000;
int seed = 260504;
var random = new Random(seed);
int[][] dataPerf = new int[SIZE][];
for (int i=0;  i<SIZE; i++)
{
    dataPerf[i] = [random.Next(1_000_000)];
}

var bigMinMaxSumLazySegmentTree = minMaxSumLazySegmentTreeFactory.CreateSegmentTree(data);
var left = dataPerf.Length * 3 / 11;
var right = dataPerf.Length * 5 / 11;
GC.TryStartNoGCRegion(30L * 1024 * 1024 * 1024);
var timer = Stopwatch.StartNew();
bigMinMaxSumLazySegmentTree.UpdateRange(10, left, right);
timer.Stop();
GC.EndNoGCRegion();
var timeInNano = ((double)timer.ElapsedTicks) / Stopwatch.Frequency * 1e9;
Console.WriteLine($"Updating {dataPerf.Length} of data takes {timeInNano} ns");
