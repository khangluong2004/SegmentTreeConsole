namespace SegmentTreeConsole;

public interface ISegmentTreeNode<T>
{
    public (int, int) GetRange();
    public T[] GetAttributesRef();
    public T[] GetLazyAttributesRef();
    /* null = reset, [] = keep unchanged */
    public void UpdateAttributes(T[]? attributeUpdates, T[]? lazyAttributes);
    
}