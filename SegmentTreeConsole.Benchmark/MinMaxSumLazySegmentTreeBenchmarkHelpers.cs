using SegmentTreeConsole.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SegmentTreeConsole.Benchmark
{
    public static class MinMaxSumLazySegmentTreeBenchmarkHelpers
    {
        public static void SetupDataAndTree(
            MinMaxSumLazySegmentTreeFactory minMaxSumLazySegmentTreeFactory,
            int NumData,
            ref int[][] data, ref GenericSegmentTree<int[], int, int[]> minMaxSumLazySegmentTree)
        {
            data = new int[NumData][];
            Random random = new(260504);
            for (int i = 0; i < NumData; i++)
            {
                data[i] = [random.Next(1000)];
            }
            minMaxSumLazySegmentTree = minMaxSumLazySegmentTreeFactory.CreateSegmentTree(data);
        }
    }
}
