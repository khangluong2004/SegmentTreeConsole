using NUnit.Framework;
using SegmentTreeConsole;
using SegmentTreeConsole.Factory;

namespace SegmentTreeConsole.Tests;

[TestFixture]
public class MinMaxSumLazySegmentTreeTest
{
    private GenericSegmentTree<int[], int, int[]> _minMaxSumLazySegmentTree;
    private readonly int[][] _data = [[2], [1], [3], [4], [5], [7], [8], [9]];
    private readonly MinMaxSumLazySegmentTreeFactory factory = new();
    [SetUp]
    public void Setup()
    {
        _minMaxSumLazySegmentTree = factory.CreateSegmentTree(_data);
    }

    [Test]
    public void TestElementQuery()
    {
        var secondLeaf = _minMaxSumLazySegmentTree.QueryRange(1, 1);
        Assert.That(secondLeaf[0], Is.EqualTo(_data[1][0]), "The second item in data need to be queried correctly");
    }
}
