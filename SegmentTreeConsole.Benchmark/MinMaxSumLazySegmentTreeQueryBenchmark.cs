using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using SegmentTreeConsole.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SegmentTreeConsole.Benchmark
{
    public class MinMaxSumLazySegmentTreeQueryBenchmark
    {
        private readonly MinMaxSumLazySegmentTreeFactory minMaxSumLazySegmentTreeFactory = new();
        private const int N = 100_000_000;
        private readonly int[][] data;
        private GenericSegmentTree<int[], int, int[]> minMaxSumLazySegmentTree;
        private readonly Consumer consumer = new();

        public MinMaxSumLazySegmentTreeQueryBenchmark()
        {
            MinMaxSumLazySegmentTreeBenchmarkHelpers.SetupDataAndTree(minMaxSumLazySegmentTreeFactory, N,
                ref data, ref minMaxSumLazySegmentTree);
        }

        private const int leftQuery = N * 3 / 11;
        private const int rightQuery = N * 9 / 11;
        [Benchmark]
        public int[] QueryRangeTest()
        {
            return minMaxSumLazySegmentTree.QueryRange(leftQuery, rightQuery);
        }
    }
}
