using SegmentTreeConsole;

namespace SegmentTreeConsole.Factory;

public enum MinMaxSumAttributeType
{
    Min = 0,
    Max = 1,
    Sum = 2
}

public enum SumLazyAttributeType
{
    Sum=0
}

public class MinMaxSumLazySegmentTreeNode<TValue, TLazy>: ISegmentTreeNode<TValue[], TLazy[]>, ISegmentTreeNodeAttributeIndexable<MinMaxSumAttributeType, SumLazyAttributeType>
{
    // Attributes: 0 = min, 1 = max, 2 = sum, 3 = lazy atributes
    private readonly TValue[] _attributes;
    private readonly TLazy[] _lazyAttributes;
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

    public (int, int) GetRange()
    {
        return (_leftRange, _rightRange);
    }
    
    public TValue[] GetAttributesRef()
    {
        return _attributes;
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

    public TLazy[] GetLazyAttributesRef()
    {
        return _lazyAttributes;
    }

    static public int GetLazyAttributeIndex(SumLazyAttributeType attributeType)
    {
        return attributeType switch
        {
            SumLazyAttributeType.Sum => 0,
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
}