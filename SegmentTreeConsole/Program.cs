using SegmentTreeConsole;
using SegmentTreeConsole.Factory;

// Sample data for the sum segment tree
int[] data = { 2, 1, 3, 4, 5, 7, 8, 9 };

Console.WriteLine("=== SUM SEGMENT TREE DEMO (using existing generic API) ===");
Console.WriteLine("Input array:");

// Made a lazy segment tree, with range query min, max, sum, and support range updates for sum
var lazyAttributesUpdater = (ISegmentTreeNode<int> curNode, int?[] lazyUpdates) =>
{
    // Sum lazy update
    var lazyAttributesRef = curNode.GetLazyAttributesRef();
    var sumLazyIndex = MinMaxSumLazySegmentTreeNode<int>.GetLazyAttributeIndex(SumLazyAttributeType.Sum);
    if (lazyUpdates[sumLazyIndex] != null)
    {
        lazyAttributesRef[sumLazyIndex] += ((int)lazyUpdates[sumLazyIndex]!);
    }

    // Sum update on the range
    var attributesRef = curNode.GetAttributesRef();
    var sumIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    (int left, int right) = curNode.GetRange();
    if (lazyUpdates[sumLazyIndex] != null)
    {
        attributesRef[sumIndex] += (((int)lazyUpdates[sumLazyIndex]!) * (right - left + 1));
    }

    return curNode;
};

var attributesUpdater = (ISegmentTreeNode<int> curNode, int?[] updates) =>
{
    var attributesRef = curNode.GetAttributesRef();
    // Min update
    var minIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Min);
    if (updates[minIndex] != null)
    {
        attributesRef[minIndex] = Math.Min(attributesRef[minIndex], (int) updates[minIndex]!);
    }    
    
    // Max update
    var maxIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Max);
    if (updates[maxIndex] != null)
    {
        attributesRef[maxIndex] = Math.Max(attributesRef[maxIndex], (int) updates[maxIndex]!);
    }

    // Lazy update the sum
    var sumIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    if (updates[sumIndex] != null)
    {
        int?[] lazyUpdates = new int?[curNode.GetLazyAttributesRef().Length];
        var sumLazyIndex = MinMaxSumLazySegmentTreeNode<int>.GetLazyAttributeIndex(SumLazyAttributeType.Sum);
        lazyUpdates[sumLazyIndex] = updates[sumIndex];
        curNode = lazyAttributesUpdater(curNode, lazyUpdates);
    }

    return curNode;
};



var MinMaxSumLazySegmentTree = new GenericSegmentTree<int>(
        data,
        attributesUpdater: attributesUpdater
);