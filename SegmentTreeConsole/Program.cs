using SegmentTreeConsole;
using SegmentTreeConsole.Factory;

// Sample data for the sum segment tree
int[][] data = [[2], [1], [3], [4], [5], [7], [8], [9]];

Console.WriteLine("=== SUM SEGMENT TREE DEMO (using existing generic API) ===");
Console.WriteLine($"Input array: {string.Join(", ", data.Select(elem => elem[0]))}");

// Made a lazy segment tree, with range query min, max, sum, and support range updates for sum
var lazyAttributesUpdater = (ISegmentTreeNode<int[], int[]> curNode, int[] lazyUpdates) =>
{
    // Sum lazy update
    var lazyAttributesRef = curNode.GetLazyAttributesRef();
    var sumLazyIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetLazyAttributeIndex(SumLazyAttributeType.Sum);
    lazyAttributesRef[sumLazyIndex] += ((int)lazyUpdates[sumLazyIndex]);

    // Sum update on the range
    var attributesRef = curNode.GetAttributesRef();
    var sumIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    (int left, int right) = curNode.GetRange();
    attributesRef[sumIndex] += (((int)lazyUpdates[sumLazyIndex]) * (right - left + 1));

    return curNode;
};

var attributesUpdater = (ISegmentTreeNode<int[], int[]> curNode, int update) =>
{
    var attributesRef = curNode.GetAttributesRef();
    var (leftRange, rightRange) = curNode.GetRange();

    if (leftRange == rightRange)
    {
        // Leaf node
        int rawIndex = MinMaxSumLazySegmentTreeLeaf<int, int>.GetAttributeIndex(MinMaxSumLazySegmentTreeLeafAttributeType.Raw);
        attributesRef[rawIndex] += update;
        Console.WriteLine($"Update leaf from {attributesRef[rawIndex] - update} to {attributesRef[rawIndex]}");
        return curNode;
    }
    

    // Otherwise tree node
    // Lazy update the sum
    var sumIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    int[] lazyUpdates = new int[curNode.GetLazyAttributesRef().Length];
    var sumLazyIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetLazyAttributeIndex(SumLazyAttributeType.Sum);
    lazyUpdates[sumLazyIndex] = update;
    curNode = lazyAttributesUpdater(curNode, lazyUpdates);

    return curNode;
};

// TODO: Fix this
var childrenNodesCombinator = (ISegmentTreeNode<int[], int[]> parent, ISegmentTreeNode<int[], int[]> leftChild, ISegmentTreeNode<int[], int[]> rightChild) =>
{
    // Parent and children nodes are reasonably assumed to have the same attributes structure
    var parentAttributesRef = parent.GetAttributesRef();
    var leftAttributesRef = leftChild.GetAttributesRef();
    var rightAttributesRef = rightChild.GetAttributesRef();

    // Min update
    var minIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Min);
    parentAttributesRef[minIndex] = Math.Min(rightAttributesRef[minIndex], leftAttributesRef[minIndex]);

    // Max update
    var maxIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Max);
    parentAttributesRef[maxIndex] = Math.Max(rightAttributesRef[maxIndex], leftAttributesRef[maxIndex]);

    // Sum update
    var sumIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
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
    var minIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Min);
    result[minIndex] = Math.Min(left[minIndex], right[minIndex]);

    // Max
    var maxIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Max);
    result[maxIndex] = Math.Max(left[maxIndex], right[maxIndex]);

    // Sum
    var sumIndex = MinMaxSumLazySegmentTreeNode<int, int>.GetAttributeIndex(MinMaxSumAttributeType.Sum);
    result[sumIndex] = left[sumIndex] + right[sumIndex];

    return result;
};

Func<int[], bool, int, int, ISegmentTreeNode<int[], int[]>> treeNodeCreator = (int[] value, bool isLeaf, int leftRange, int rightRange) =>
{
    if (!isLeaf)
    {
        return new MinMaxSumLazySegmentTreeNode<int, int>([Int32.MaxValue, Int32.MinValue, 0], [0],
            [Int32.MaxValue, Int32.MinValue, 0], [0], leftRange, rightRange);
    }

    return new MinMaxSumLazySegmentTreeLeaf<int, int>([(int)value[0]], [0], leftRange, rightRange);
};

var MinMaxSumLazySegmentTree = new GenericSegmentTree<int[], int, int[]>(
        data,
        attributesUpdater: attributesUpdater,
        lazyAttributesUpdater: lazyAttributesUpdater,
        childrenNodesCombinator: childrenNodesCombinator,
        queryAttributesCombinator: queryAttributesCombinator,
        treeNodeCreator: treeNodeCreator
);

Console.WriteLine($"{string.Join(", ", MinMaxSumLazySegmentTree.QueryRange(0, 3)!)}");
Console.WriteLine($"{string.Join(", ", MinMaxSumLazySegmentTree.QueryRange(2, 5)!)}");
// TODO: Change the update. Should just be update to the data (so no array of updates).
// Debug update
MinMaxSumLazySegmentTree.UpdateRange(2, 0, 3);
Console.WriteLine($"{string.Join(", ", MinMaxSumLazySegmentTree.QueryRange(0, 3)!)}");
