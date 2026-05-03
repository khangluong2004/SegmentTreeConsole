namespace SegmentTreeConsole;

public interface ISegmentTreeNode<T>
{
    public (int, int) GetRange();
    public T[] GetAttributesRef();
    /* null = reset, [] = keep unchanged */
    public T[] GetLazyAttributesRef();
    public void UpdateAttributes(T[]? attributeUpdates);
    public void UpdateLazyAttributes(T[]? lazyAttributeUpdates);
}

public interface ISegmentTreeNodeAttributeIndexable<AttributeEnum, LazyAttributeEnum>
    where AttributeEnum : Enum
    where LazyAttributeEnum : Enum
{
    public static abstract int GetAttributeIndex(AttributeEnum attribute);
    public static abstract int GetLazyAttributeIndex(LazyAttributeEnum lazyAttribute);
}