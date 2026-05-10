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

public class MinMaxSumLazySegmentTreeNode<T>: ISegmentTreeNode<T>, ISegmentTreeNodeAttributeIndexable<MinMaxSumAttributeType, SumLazyAttributeType>
{
    // Attributes: 0 = min, 1 = max, 2 = sum, 3 = lazy atributes
    private readonly T[] _attributes;
    private readonly T[] _lazyAttributes;
    private readonly int _leftRange, _rightRange;
    private readonly T[] _defaultAttributes;
    private readonly T[] _defaultLazyAttributes;

    public MinMaxSumLazySegmentTreeNode(T[] attributes, T[] lazyAttributes, 
        T[] defaultAttributes, T[] defaultLazyAttributes,
        int leftRange, int rightRange)
    {
        _attributes = new T[attributes.Length];
        Array.Copy(attributes, _attributes, attributes.Length);
        _lazyAttributes = new T[lazyAttributes.Length];
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
    
    public T[] GetAttributesRef()
    {
        return _attributes;
    }
    

    public void UpdateAttributes(T[]? attributeUpdates)
    {
        if (attributeUpdates != null)
        {
            Array.Copy(attributeUpdates, _attributes, attributeUpdates.Length);
        } else
        {
            Array.Copy(_defaultAttributes, _attributes, _defaultAttributes.Length);
        }
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

    public T[] GetLazyAttributesRef()
    {
        return _lazyAttributes;
    }

    public void UpdateLazyAttributes(T[]? lazyAttributeUpdates)
    {
        if (lazyAttributeUpdates != null)
        {
            Array.Copy(lazyAttributeUpdates, _lazyAttributes, _lazyAttributes.Length);
        }
        else
        {
            Array.Copy(_defaultLazyAttributes, _lazyAttributes, _defaultLazyAttributes.Length);
        }
    }

    static public int GetLazyAttributeIndex(SumLazyAttributeType attributeType)
    {
        return attributeType switch
        {
            SumLazyAttributeType.Sum => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(attributeType), attributeType, null),
        };
    }
}