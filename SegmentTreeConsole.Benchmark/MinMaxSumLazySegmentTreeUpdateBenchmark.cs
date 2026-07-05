using BenchmarkDotNet.Attributes;
using SegmentTreeConsole.Factory;
using BenchmarkDotNet.Engines;


namespace SegmentTreeConsole.Benchmark
{

    public class MinMaxSumLazySegmentTreeUpdateBenchmark
    {
        private readonly MinMaxSumLazySegmentTreeFactory minMaxSumLazySegmentTreeFactory = new();
        private const int N = 100_000_000;
        private readonly int[][] data;
        private GenericSegmentTree<int[], int, int[]> minMaxSumLazySegmentTree;
        private readonly Consumer consumer = new();

        public MinMaxSumLazySegmentTreeUpdateBenchmark()
        {
            MinMaxSumLazySegmentTreeBenchmarkHelpers.SetupDataAndTree(minMaxSumLazySegmentTreeFactory, N, 
                ref data, ref minMaxSumLazySegmentTree);
        }

        private const int leftUpdate = N * 3 / 11;
        private const int rightUpdate = N * 5 / 11;
        private const int update = 31;
        [IterationCleanup]
        public void CleanupTree()
        {
            minMaxSumLazySegmentTree.UpdateRange(-update, leftUpdate, rightUpdate);
        }

        [Benchmark]
        public void UpdateTreeTest()
        {
            minMaxSumLazySegmentTree.UpdateRange(update, leftUpdate, rightUpdate);

            // Use the consumer to put a volatile barrier 
            // to avoid dead code elimination
            consumer.Consume(minMaxSumLazySegmentTree);
        }
    }
}
