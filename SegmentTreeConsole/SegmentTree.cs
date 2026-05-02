namespace SegmentTreeConsole;

public class SegmentTree<T>
{
    public readonly int DataLength;
    private ISegmentTreeNode<T>[] TreeData { get;  }
    private Func<ISegmentTreeNode<T>, T[], ISegmentTreeNode<T>> AttributesUpdater { get;  }
    private Func<ISegmentTreeNode<T>, ISegmentTreeNode<T>, 
        ISegmentTreeNode<T>, ISegmentTreeNode<T>> ChildrenNodesCombinator { get;  }
    private Func<T[]?, T[]?, T[]?> QueryAttributesCombinator { get;  }
    
    private Func<T?, int, int, ISegmentTreeNode<T>> TreeNodeCreator { get;  }
    
    public SegmentTree(T[] data, 
        Func<ISegmentTreeNode<T>, T[], ISegmentTreeNode<T>> attributesUpdater,
        Func<ISegmentTreeNode<T>, ISegmentTreeNode<T>, 
            ISegmentTreeNode<T>, ISegmentTreeNode<T>> childrenNodesCombinator,
        Func<T[]?, T[]?, T[]?> queryAttributesCombinator,
        Func<ISegmentTreeNode<T>, ISegmentTreeNode<T>> lazyAttributeReset,
        Func<T?, int, int, ISegmentTreeNode<T>> treeNodeCreator)
    {
        TreeData = new ISegmentTreeNode<T>[data.Length * 4];
        
        AttributesUpdater = attributesUpdater;
        ChildrenNodesCombinator = childrenNodesCombinator;
        TreeNodeCreator = treeNodeCreator;
        LazyAttributeReset = lazyAttributeReset;
        DataLength = data.Length;
        QueryAttributesCombinator = queryAttributesCombinator;
        
        this.Build(data, 1, 0,  data.Length - 1);
    }

    private void Build(T[] data, int vertex, int left, int right)
    {
        if (left == right)
        {
            TreeData[vertex] = TreeNodeCreator(data[left], left, right);
        }
        else
        {
            var mid = (left + right) / 2;
            this.Build(data, vertex * 2, left, mid);
            this.Build(data, vertex * 2 + 1, mid + 1, right);
            TreeData[vertex] =  TreeNodeCreator(default, left, right);
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
        var leftChild = TreeData[vertex * 2];
        
        TreeData[vertex*2] = AttributesUpdater(leftChild, parent.GetLazyAttributesRef());
        
        
        var rightChild = TreeData[vertex * 2 + 1];
        TreeData[vertex * 2 + 1] = AttributesUpdater(rightChild, parent.GetLazyAttributesRef());
        
        
        parent.UpdateAttributes([], null);
    }

    private void UpdateRangeHelper(T[] updates, int leftUpdate, int rightUpdate, 
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

    public void UpdateRange(T[] updates, int leftUpdate, int rightUpdate)
    {
        UpdateRangeHelper(updates, leftUpdate, rightUpdate, 1, 0, DataLength - 1);
    }

    private T[]? QueryRangeHelper(int leftQuery, int rightQuery, int vertex, int leftTree, int rightTree)
    {
        if (leftQuery > rightTree || rightQuery < leftTree)
        {
            return null;
        }
        
        if (leftTree == leftQuery && rightTree == rightQuery)
        {
            return TreeData[vertex].GetAttributesRef();
        }
        
        this.PushLazy(vertex);
        var midTree = (leftTree + rightTree) / 2;
        var resultLeft = QueryRangeHelper(leftQuery, Math.Min(midTree, rightQuery), vertex, leftTree, midTree);
        var resultRight = QueryRangeHelper(Math.Max(leftQuery, midTree+1), rightQuery, vertex, midTree+1, rightTree);
        
        return QueryAttributesCombinator(resultLeft, resultRight);
    }

    public T[]? QueryRange(int leftQuery, int rightQuery)
    {
        return QueryRangeHelper(leftQuery, rightQuery, 1, 0, DataLength - 1);
    }
    
}