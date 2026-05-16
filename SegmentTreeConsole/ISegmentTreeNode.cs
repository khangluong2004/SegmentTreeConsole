namespace SegmentTreeConsole;

public interface ISegmentTreeNode<TValue, TLazy>
{
    public (int, int) GetRange();
    public TValue GetAttributesRef();
    /* null = reset, [] = keep unchanged */
    public TLazy GetLazyAttributesRef();

    public void ResetLazyAttributes();
    public void ResetAttributes();
}

public interface ISegmentTreeNodeAttributeIndexable<AttributeEnum, LazyAttributeEnum>
    where AttributeEnum : Enum
    where LazyAttributeEnum : Enum
{
    public static abstract int GetAttributeIndex(AttributeEnum attribute);
    public static abstract int GetLazyAttributeIndex(LazyAttributeEnum lazyAttribute);
}