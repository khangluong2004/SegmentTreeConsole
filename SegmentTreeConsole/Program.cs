using SegmentTreeConsole;
using SegmentTreeConsole.Factory;

// Sample data for the sum segment tree
int[] data = { 2, 1, 3, 4, 5, 7, 8, 9 };

Console.WriteLine("=== SUM SEGMENT TREE DEMO (using existing generic API) ===");
Console.WriteLine("Input array:");

// Made a lazy segment tree, with range query min, max, sum, and support range updates for sum
Func<ISegmentTreeNode<int>, int[], ISegmentTreeNode<int>> lazyAttributesUpdater = (ISegmentTreeNode<int> curNode, int?[] lazyUpdates) =>
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

Func<ISegmentTreeNode<int>, int[], ISegmentTreeNode<int>> attributesUpdater = (ISegmentTreeNode<int> curNode, int?[] updates) =>
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

var childrenNodesCombinator = (ISegmentTreeNode<int> parent, ISegmentTreeNode<int> leftChild, ISegmentTreeNode<int> rightChild) =>
{
    // Parent and children nodes are reasonably assumed to have the same attributes structure
    var parentAttributesRef = parent.GetAttributesRef();
    var leftAttributesRef = leftChild.GetAttributesRef();
    var rightAttributesRef = rightChild.GetAttributesRef();

    // Min update
    var minIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Min);
    parentAttributesRef[minIndex] = Math.Min(rightAttributesRef[minIndex], leftAttributesRef[minIndex]);

    // Max update
    var maxIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Max);
    parentAttributesRef[maxIndex] = Math.Max(rightAttributesRef[maxIndex], leftAttributesRef[maxIndex]);

    // Sum update
    var sumIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    parentAttributesRef[sumIndex] = leftAttributesRef[sumIndex] + rightAttributesRef[sumIndex];

    return parent;
};

var queryAttributesCombinator = (int[]? left, int[]? right) =>
{
    if (left == null)
    {
        return right;
    }

    if (right == null) {
        return left;
    }

    int[] result = new int[left.Length];
    // Min
    var minIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Min);
    result[minIndex] = Math.Min(left[minIndex], right[minIndex]);

    // Max
    var maxIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Max);
    result[maxIndex] = Math.Max(left[maxIndex], right[maxIndex]);

    // Sum
    var sumIndex = MinMaxSumLazySegmentTreeNode<int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    result[sumIndex] = left[sumIndex] + right[sumIndex];

    return result;
};

Func<int?, int, int, ISegmentTreeNode<int>> treeNodeCreator = (int? value, int leftRange, int rightRange) =>
{
    if (value == null)
    {
        return new MinMaxSumLazySegmentTreeNode<int>([Int32.MaxValue, Int32.MinValue, 0], [0],
            [Int32.MaxValue, Int32.MinValue, 0], [0], leftRange, rightRange);
    }

    return new MinMaxSumLazySegmentTreeNode<int>([(int) value, (int) value, (int) value], [0], 
        [Int32.MaxValue, Int32.MinValue, 0], [0], leftRange, rightRange);
};

var MinMaxSumLazySegmentTree = new GenericSegmentTree<int>(
        data,
        attributesUpdater: attributesUpdater,
        lazyAttributesUpdater: lazyAttributesUpdater,
        childrenNodesCombinator: childrenNodesCombinator,
        queryAttributesCombinator: queryAttributesCombinator,
        treeNodeCreator: treeNodeCreator
);