using SegmentTreeConsole;

// Sample data for the sum segment tree
int[] data = { 2, 1, 3, 4, 5, 7, 8, 9 };

Console.WriteLine("=== SUM SEGMENT TREE DEMO (using existing generic API) ===");
Console.WriteLine("Input array:");
for (int i = 0; i < data.Length; i++)
{
    Console.WriteLine($"  index {i}: {data[i]}");
}
Console.WriteLine();

// We will use:
// - Node attributes: [sum] only (int[1])
// - No lazy behavior for now (we won't call UpdateRange)
// This keeps the usage simple and focused on range-sum queries.

SegmentTree<int> sumTree = new SegmentTree<int>(
    data,
    // attributesUpdater: for this demo we don't use lazy updates,
    // so just return the node unchanged.
    attributesUpdater: (node, updates) =>
    {
        // No-op lazy update in this sample
        return node;
    },
    // childrenNodesCombinator: combine two children into the parent.
    // Each node stores a single attribute: total sum of its segment.
    childrenNodesCombinator: (parent, leftChild, rightChild) =>
    {
        var leftAttrs = leftChild.GetAttributesRef();
        var rightAttrs = rightChild.GetAttributesRef();

        int[] combined = new int[1];
        combined[0] = leftAttrs[0] + rightAttrs[0];

        // We don't track lazy attributes in this simple sum tree demo.
        parent.UpdateAttributes(combined, null);
        return parent;
    },
    // queryAttributesCombinator: combine partial sums from left and right queries.
    queryAttributesCombinator: (leftAttrs, rightAttrs) =>
    {
        if (leftAttrs is null) return rightAttrs;
        if (rightAttrs is null) return leftAttrs;

        int[] combined = new int[1];
        combined[0] = leftAttrs[0] + rightAttrs[0];
        return combined;
    },
    // lazyAttributeReset: not used in this demo; just return node unchanged.
    lazyAttributeReset: node =>
    {
        // No lazy attributes in this simple setup
        return node;
    },
    // treeNodeCreator: creates a concrete SegmentTreeNode<int> for each segment.
    treeNodeCreator: (value, left, right) =>
    {
        int v = value;

        // Single attribute: sum over [left, right]. For leaf nodes this is just v.
        int[] attributes = { v };

        // No lazy attributes in this simple example.
        int[] lazyAttributes = Array.Empty<int>();

        return new SegmentTreeNode<int>(
            attributes,
            lazyAttributes,
            left,
            right
        );
    }
);

// For now, because the current generic implementation's query helper is only
// safe for the full range [0, n-1], we demonstrate the sum over the whole array.

Console.WriteLine("=== QUERY: full range sum [0, n-1] ===");
int[]? treeResult = sumTree.QueryRange(0, data.Length - 1);

int expectedTotal = 0;
for (int i = 0; i < data.Length; i++)
{
    expectedTotal += data[i];
}

Console.WriteLine($"Segment tree reported sum: {treeResult?[0]}");
Console.WriteLine($"Expected (naive) sum    : {expectedTotal}");
Console.WriteLine();
Console.WriteLine("You can now extend this demo with more queries/updates as you evolve the generic tree implementation.");
Console.WriteLine();

// Additional tests to inspect current behavior on partial range queries.
int NaiveSum(int[] source, int l, int r)
{
    int sum = 0;
    for (int i = l; i <= r; i++)
    {
        sum += source[i];
    }

    return sum;
}

// Test a few partial range queries to see how the current implementation behaves.
var testRanges = new (int L, int R)[]
{
    (0, 0),
    (0, 3),
    (2, 5)
};

foreach (var (L, R) in testRanges)
{
    Console.WriteLine($"=== QUERY: partial range [{L}, {R}] ===");
    try
    {
        var result = sumTree.QueryRange(L, R);
        var expected = NaiveSum(data, L, R);
        Console.WriteLine($"Segment tree reported sum: {result?[0]}");
        Console.WriteLine($"Expected (naive) sum    : {expected}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Query [{L}, {R}] threw {ex.GetType().Name}: {ex.Message}");
    }

    Console.WriteLine();
}

// Test a simple range update plus a query to observe lazy propagation behavior.
Console.WriteLine("=== RANGE UPDATE + QUERY (current behavior) ===");
try
{
    int[] update = { 5 }; // semantics are undefined in the current sample; this is only to drive the code path
    sumTree.UpdateRange(update, 2, 5);

    var afterUpdate = sumTree.QueryRange(0, data.Length - 1);
    int expectedAfterUpdate = 0;
    for (int i = 0; i < data.Length; i++)
    {
        expectedAfterUpdate += data[i];
    }

    Console.WriteLine($"Segment tree reported sum after update: {afterUpdate?[0]}");
    Console.WriteLine($"Expected (naive, no-op assumption)   : {expectedAfterUpdate}");
}
catch (Exception ex)
{
    Console.WriteLine($"Range update or subsequent query threw {ex.GetType().Name}: {ex.Message}");
}