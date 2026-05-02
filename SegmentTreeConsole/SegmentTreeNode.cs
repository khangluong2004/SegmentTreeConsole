using SegmentTreeConsole;

namespace PlayGroundConsole;


public class SegmentTreeNode<T>: ISegmentTreeNode<T>
{
    // Attributes: 0 = min, 1 = max, 2 = sum
    private readonly T[] _attributes;
    private readonly T[] _lazyAttributes;
    private readonly int _leftRange, _rightRange;
    
    public SegmentTreeNode(T[] attributes, T[] lazyAttributes, 
        int leftRange, int rightRange)
    {
        _attributes = new T[attributes.Length];
        Array.Copy(attributes, _attributes, attributes.Length);
        _lazyAttributes = lazyAttributes;
        Array.Copy(lazyAttributes, _lazyAttributes, _lazyAttributes.Length);
        this._leftRange = leftRange;
        this._rightRange = rightRange;
    }

    public (int, int) GetRange()
    {
        return (_leftRange, _rightRange);
    }
    
    public T[] GetAttributesRef()
    {
        // Return a copy of the array
        return _attributes;
    }
    
    public T[] GetLazyAttributesRef()
    {
        // Return a copy of the array
        return _lazyAttributes;
    }
    

    public void UpdateAttributes(T[]? attributeUpdates, T[]? lazyAttributes)
    {
        if (attributeUpdates != null)
        {
            for (var i = 0; i < _attributes.Length; i++)
            {
                _attributes[i] = attributeUpdates[i];
            }
        }

        if (lazyAttributes == null) return;

        for (var i = 0; i < lazyAttributes.Length; i++)
        {
            _lazyAttributes[i] = lazyAttributes[i];
        }
        
    }
}