using SegmentTreeConsole;
using SegmentTreeConsole.Factory;

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

// Bug found: Expect sum = 20, got 16
minMaxSumLazySegmentTree.UpdateRange(2, 3, 5);
Console.WriteLine($"{string.Join(", ", minMaxSumLazySegmentTree.QueryRange(0, 3)!)}");

