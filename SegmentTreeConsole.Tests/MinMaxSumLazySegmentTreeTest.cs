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
        var index = Random.Shared.Next(0, _data.Length);
        var minMaxSumResult = _minMaxSumLazySegmentTree.QueryRange(index, index);
        var minIndex = (int) MinMaxSumAttributeType.Min;
        Assert.That(minMaxSumResult[minIndex], Is.EqualTo(_data[index][0]), "The data = min of 1 element");
        var maxIndex = (int) MinMaxSumAttributeType.Max;
        Assert.That(minMaxSumResult[maxIndex], Is.EqualTo(_data[index][0]), "The data = max of 1 element");
        var sumIndex = (int) MinMaxSumAttributeType.Sum;
        Assert.That(minMaxSumResult[sumIndex], Is.EqualTo(_data[index][0]), "The data = sum of 1 element");
    }

    [Test]
    public void TestRangeQuery()
    {
        var left = Random.Shared.Next(0, Math.Max(0, _data.Length - 2));
        var right = Random.Shared.Next(left + 1, _data.Length);
        var minMaxSumResult = _minMaxSumLazySegmentTree.QueryRange(left, right);
        var minIndex = (int)MinMaxSumAttributeType.Min;
        var flattenSection = _data.Skip(left).Take(right - left + 1).Select(e => e[0]);
        var realMinVal = flattenSection.Min(); 
        Assert.That(minMaxSumResult[minIndex], Is.EqualTo(realMinVal), $"The min of [{string.Join(", ", flattenSection)}] is {realMinVal}");
        var maxIndex = (int)MinMaxSumAttributeType.Max;
        var realMaxVal = flattenSection.Max();
        Assert.That(minMaxSumResult[maxIndex], Is.EqualTo(realMaxVal), $"The max of [{string.Join(", ", flattenSection)}] is {realMaxVal}");
        var sumIndex = (int)MinMaxSumAttributeType.Sum;
        var realSumVal = flattenSection.Sum();
        Assert.That(minMaxSumResult[sumIndex], Is.EqualTo(realSumVal), $"The sum of [{string.Join(", ", flattenSection)}] is {realSumVal}");
    }

    [Test]
    public void TestRangeUpdate_QueryRange()
    {
        var update = Random.Shared.Next(-10, 10);
        var left = Random.Shared.Next(0, Math.Max(0, _data.Length - 2));
        var right = Random.Shared.Next(left + 1, _data.Length);
        _minMaxSumLazySegmentTree.UpdateRange(update, left, right);

        var flattenSection = _data.Skip(left).Take(right - left + 1).Select(e => e[0] + 5);

        var minMaxSumResult = _minMaxSumLazySegmentTree.QueryRange(left, right);
        var minIndex = (int)MinMaxSumAttributeType.Min;
        var expectedMinVal = flattenSection.Min();
        Assert.That(minMaxSumResult[minIndex], Is.EqualTo(expectedMinVal), $"Min of [{string.Join(", ", flattenSection)}] after update is {expectedMinVal}");
        var maxIndex = (int)MinMaxSumAttributeType.Max;
        var expectedMaxVal = flattenSection.Max();
        Assert.That(minMaxSumResult[maxIndex], Is.EqualTo(expectedMaxVal), $"Max of [{string.Join(", ", flattenSection)}] after update is {expectedMaxVal}");
        var sumIndex = (int)MinMaxSumAttributeType.Sum;
        var expectedSumVal = flattenSection.Sum();
        Assert.That(minMaxSumResult[sumIndex], Is.EqualTo(expectedSumVal), $"Sum of [{string.Join(", ", flattenSection)}] after update is {expectedSumVal}");
    }


    [Test]
    public void TestRangeUpdate_QueryEach()
    {
        var update = Random.Shared.Next(-10, 10);
        var left = Random.Shared.Next(0, Math.Max(0, _data.Length - 2));
        var right = Random.Shared.Next(left + 1, _data.Length);
        _minMaxSumLazySegmentTree.UpdateRange(update, left, right);

        for (int i=0; i < _data.Length; i++)
        {
            var localUpdate = 0;
            if (left <= i && i <= right)
            {
                localUpdate = update;
            }
            var minMaxSumResult = _minMaxSumLazySegmentTree.QueryRange(i, i);
            var minIndex = (int)MinMaxSumAttributeType.Min;
            var expectedMinVal = _data[i][0] + localUpdate;
            Assert.That(minMaxSumResult[minIndex], Is.EqualTo(expectedMinVal), $"Min after update is {expectedMinVal}");
            var maxIndex = (int)MinMaxSumAttributeType.Max;
            var expectedMaxVal = _data[i][0] + localUpdate;
            Assert.That(minMaxSumResult[maxIndex], Is.EqualTo(expectedMaxVal), $"Max after update is {expectedMaxVal}");
            var sumIndex = (int)MinMaxSumAttributeType.Sum;
            var expectedSumVal = _data[i][0] + localUpdate;
            Assert.That(minMaxSumResult[sumIndex], Is.EqualTo(expectedSumVal), $"Sum after update is {expectedSumVal}");
        }
        
    }

}
