namespace SegmentTreeConsole;

public interface ISegmentTreeNode<TValue, TLazy>
{
    public (int, int) GetRange();
    public ref TValue GetAttributesRef();
    public TValue GetQueryAttributesRef(); // Might return a ref for performance. Need to be copied before mutating.
    public ref TLazy GetLazyAttributesRef();

    public void ResetLazyAttributes();
    public void ResetAttributes();

    public bool IsLeaf();
}

public interface ISegmentTreeNodeAttributeIndexable<AttributeEnum, LazyAttributeEnum>
    where AttributeEnum : Enum
    where LazyAttributeEnum : Enum
{
    public static abstract int GetAttributeIndex(AttributeEnum attribute);
    public static abstract int GetLazyAttributeIndex(LazyAttributeEnum lazyAttribute);
}