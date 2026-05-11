namespace SegmentTreeConsole;

public class GenericSegmentTree<TValue, TUpdate>
{
    // The left-right range are inclusive
    public readonly int DataLength;
    private ISegmentTreeNode<TValue>[] TreeData { get;  }
    private Func<TValue, bool, int, int, ISegmentTreeNode<TValue>> TreeNodeCreator { get; }
    private Func<ISegmentTreeNode<TValue>, TUpdate[], ISegmentTreeNode<TValue>> AttributesUpdater { get;  }
    private Func<ISegmentTreeNode<TValue>, TValue[], ISegmentTreeNode<TValue>> LazyAttributesUpdater { get; }
    private Func<ISegmentTreeNode<TValue>, ISegmentTreeNode<TValue>, 
        ISegmentTreeNode<TValue>, ISegmentTreeNode<TValue>> ChildrenNodesCombinator { get;  }
    private Func<TValue[]?, TValue[]?, TValue[]?> QueryAttributesCombinator { get;  }
    
    
    public GenericSegmentTree(TValue[] data, 
        Func<ISegmentTreeNode<TValue>, TUpdate[], ISegmentTreeNode<TValue>> attributesUpdater,
        Func<ISegmentTreeNode<TValue>, TValue[], ISegmentTreeNode<TValue>> lazyAttributesUpdater,
        Func<ISegmentTreeNode<TValue>, ISegmentTreeNode<TValue>, 
            ISegmentTreeNode<TValue>, ISegmentTreeNode<TValue>> childrenNodesCombinator,
        Func<TValue[]?, TValue[]?, TValue[]?> queryAttributesCombinator,
        Func<TValue, bool, int, int, ISegmentTreeNode<TValue>> treeNodeCreator)
    {
        TreeData = new ISegmentTreeNode<TValue>[data.Length * 4];
        
        AttributesUpdater = attributesUpdater;
        ChildrenNodesCombinator = childrenNodesCombinator;
        TreeNodeCreator = treeNodeCreator;
        DataLength = data.Length;
        QueryAttributesCombinator = queryAttributesCombinator;
        LazyAttributesUpdater = lazyAttributesUpdater;

        if (data.Length == 0)
        {
            throw new ArgumentException("Length of data must be positive");
        }
        this.Build(data, 1, 0,  data.Length - 1);
    }

    private void Build(TValue[] data, int vertex, int left, int right)
    {
        if (left == right)
        {
            TreeData[vertex] = TreeNodeCreator(data[left], false, left, right);
        }
        else
        {
            var mid = (left + right) / 2;
            this.Build(data, vertex * 2, left, mid);
            this.Build(data, vertex * 2 + 1, mid + 1, right);
            TreeData[vertex] =  TreeNodeCreator(default, true, left, right);
            TreeData[vertex] = 
                ChildrenNodesCombinator(
                    TreeData[vertex],
                    TreeData[vertex * 2],
                    TreeData[vertex * 2 + 1]
                );
        }
    }

    private void PushLazy(int vertex)
    {
        var parent = TreeData[vertex];
        if (vertex * 2 < TreeData.Length)
        {
            var leftChild = TreeData[vertex * 2];

            TreeData[vertex * 2] = LazyAttributesUpdater(leftChild, parent.GetLazyAttributesRef());
        }
       
        
        if (vertex * 2 + 1  < TreeData.Length)
        {
            var rightChild = TreeData[vertex * 2 + 1];
            TreeData[vertex * 2 + 1] = LazyAttributesUpdater(rightChild, parent.GetLazyAttributesRef());
        }
        
        parent.UpdateLazyAttributes(null);
    }

    private void UpdateRangeHelper(TUpdate[] updates, int leftUpdate, int rightUpdate, 
        int vertex, int leftTree, int rightTree)
    {
        if (leftUpdate > rightTree || rightUpdate < leftTree)
        {
            return;
        }
        
        var curNode =  TreeData[vertex];
        if (leftUpdate <= leftTree && rightTree <= rightUpdate)
        {
            TreeData[vertex] = AttributesUpdater(curNode, updates);
        }
        else
        {
            this.PushLazy(vertex);
            var midTree = (leftTree + rightTree) / 2;
            this.UpdateRangeHelper(updates, leftUpdate, Math.Min(rightUpdate, midTree), 
                2*vertex, leftTree, midTree);
            this.UpdateRangeHelper(updates, Math.Max(leftUpdate, midTree + 1), rightUpdate, 
                2 * vertex + 1,  midTree + 1, rightTree);
            var leftChild = TreeData[vertex * 2];
            var rightChild = TreeData[vertex * 2 + 1];
            
            TreeData[vertex] = ChildrenNodesCombinator(curNode, leftChild, rightChild);
        }
    }

    public void UpdateRange(TUpdate[] updates, int leftUpdate, int rightUpdate)
    {
        UpdateRangeHelper(updates, leftUpdate, rightUpdate, 1, 0, DataLength - 1);
    }

    private TValue[]? QueryRangeHelper(int leftQuery, int rightQuery, int vertex, int leftTree, int rightTree)
    {
        if (leftQuery > rightTree || rightQuery < leftTree)
        {
            return null;
        }
        
        if (leftQuery <= leftTree && rightTree <= rightQuery)
        {
            return TreeData[vertex].GetAttributesRef();
        }
        
        this.PushLazy(vertex);
        var midTree = (leftTree + rightTree) / 2;
        var resultLeft = QueryRangeHelper(leftQuery, Math.Min(midTree, rightQuery), vertex * 2, leftTree, midTree);
        var resultRight = QueryRangeHelper(Math.Max(leftQuery, midTree+1), rightQuery, vertex * 2 + 1, midTree+1, rightTree);
        
        return QueryAttributesCombinator(resultLeft, resultRight);
    }

    public TValue[]? QueryRange(int leftQuery, int rightQuery)
    {
        return QueryRangeHelper(leftQuery, rightQuery, 1, 0, DataLength - 1);
    }
    
}