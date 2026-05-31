using SegmentTreeConsole.Factory;
using System.Diagnostics;

namespace SegmentTreeConsole.Tests;

[TestFixture]
public class MinMaxSumLazySegmentTreePerfTest
{
    private GenericSegmentTree<int[], int, int[]>[] _minMaxSumSegmentTrees;
    private readonly MinMaxSumLazySegmentTreeFactory _minMaxSumLazySegmentTreeFactory = new();
    private int[][][] _dataSets;
    private readonly int NUM_SETS = 8;

    [OneTimeSetUp]
    public void SetUp()
    {
        int seed = 260504;
        var random = new Random(seed);
        _dataSets = new int[NUM_SETS][][];
        for (int i=0; i < NUM_SETS; i++)
        {
            int size = (int)Math.Pow(10, i);
            _dataSets[i] = new int[size][];
            for (int j=0; j <size; j++)
            {
                _dataSets[i][j] = [random.Next(1_000_000_000)];
            }
        }

        _minMaxSumSegmentTrees = new GenericSegmentTree<int[], int, int[]>[NUM_SETS];

        for (int i=0; i < NUM_SETS; i++)
        {
            _minMaxSumSegmentTrees[i] = _minMaxSumLazySegmentTreeFactory.CreateSegmentTree(_dataSets[i]);
        }
    }

    [Test]
    public void TestUpdatePerf()
    {
        double[] times = new double[NUM_SETS];
        // Warm up to force JIT to compile the path
        // Run on any SINGLE tree is fine, but run on all trees to ensure no bias
        for (int i=0; i < NUM_SETS; i++)
        {
            int length = _minMaxSumSegmentTrees[i].DataLength;
            _minMaxSumSegmentTrees[i].UpdateRange(10, (length - 1) / 5, (length - 1) * 4 / 5);
        }
        

        for (int i  = 0; i < NUM_SETS; i++)
        {
            int length = _minMaxSumSegmentTrees[i].DataLength;
            var stopWatch = Stopwatch.StartNew();
            _minMaxSumSegmentTrees[i].UpdateRange(10, (length-1) / 5, (length-1) * 4 / 5);
            _minMaxSumSegmentTrees[i].UpdateRange(7, (length - 1) / 3, (length - 1) * 2 / 3);
            _minMaxSumSegmentTrees[i].UpdateRange(9, (length - 1) / 4, (length - 1) / 2);
            stopWatch.Stop();
            times[i] = ((double) stopWatch.ElapsedTicks) / Stopwatch.Frequency * 1e6;
        }

        // Ensure it's in log complexity (can make it a bit less strict to around 2 * log(n) to accommodate for C# overhead)
        for (int i = 1; i < NUM_SETS; i++) {
            var ratio = times[i] / times[0];
            var expectedCap = Math.Log(_dataSets[i].Length / _dataSets[0].Length, 2);
            Assert.That(ratio, Is.LessThan(expectedCap), $"The run time for datasets of size {_dataSets[i].Length} should be less than {expectedCap} " +
                $"compared to dataset with length {_dataSets[0].Length}");
        }
    }
}
