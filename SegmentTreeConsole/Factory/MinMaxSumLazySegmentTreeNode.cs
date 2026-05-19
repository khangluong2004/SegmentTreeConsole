using SegmentTreeConsole;

namespace SegmentTreeConsole.Factory;

public enum MinMaxSumAttributeType
{
    Min = 0,
    Max = 1,
    Sum = 2
}

public enum AddUpdateLazyAttributeType
{
    Add=0
}

public class MinMaxSumLazySegmentTreeNode<TValue, TLazy>: ISegmentTreeNode<TValue[], TLazy[]>, ISegmentTreeNodeAttributeIndexable<MinMaxSumAttributeType, AddUpdateLazyAttributeType>
{
    // Attributes: 0 = min, 1 = max, 2 = sum, 3 = lazy atributes
    private TValue[] _attributes;
    private TLazy[] _lazyAttributes;
    private readonly int _leftRange, _rightRange;
    private readonly TValue[] _defaultAttributes;
    private readonly TLazy[] _defaultLazyAttributes;

    public MinMaxSumLazySegmentTreeNode(TValue[] attributes, TValue[] lazyAttributes, 
        TValue[] defaultAttributes, TLazy[] defaultLazyAttributes,
        int leftRange, int rightRange)
    {
        _attributes = new TValue[attributes.Length];
        Array.Copy(attributes, _attributes, attributes.Length);
        _lazyAttributes = new TLazy[lazyAttributes.Length];
        Array.Copy(lazyAttributes, _lazyAttributes, lazyAttributes.Length);
        this._leftRange = leftRange;
        this._rightRange = rightRange;
        _defaultAttributes = defaultAttributes;
        _defaultLazyAttributes = defaultLazyAttributes;
    }

    public bool IsLeaf()
    {
        return false;
    }

    public (int, int) GetRange()
    {
        return (_leftRange, _rightRange);
    }
    
    public ref TValue[] GetAttributesRef()
    {
        return ref _attributes;
    }

    static public int GetAttributeIndex(MinMaxSumAttributeType attributeType)
    {
        return attributeType switch
        {
            MinMaxSumAttributeType.Min => 0,
            MinMaxSumAttributeType.Max => 1,
            MinMaxSumAttributeType.Sum => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(attributeType), attributeType, null),
        };
    }

    public ref TLazy[] GetLazyAttributesRef()
    {
        return ref _lazyAttributes;
    }

    static public int GetLazyAttributeIndex(AddUpdateLazyAttributeType attributeType)
    {
        return attributeType switch
        {
            AddUpdateLazyAttributeType.Add => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(attributeType), attributeType, null),
        };
    }

    public void ResetLazyAttributes()
    {
        Array.Copy(_defaultLazyAttributes, _lazyAttributes, _lazyAttributes.Length);
    }

    public void ResetAttributes()
    {
        Array.Copy(_defaultAttributes, _attributes, _attributes.Length);
    }

    public TValue[] GetQueryAttributesRef()
    {
        return _attributes;
    }
}