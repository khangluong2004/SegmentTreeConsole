using SegmentTreeConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SegmentTreeConsole.Factory
{
    public enum MinMaxSumLazySegmentTreeLeafAttributeType
    {
        Raw = 0
    }

    public enum MinMaxSumLazySegmentTreeLeafLazyAttributeType { }

    public class MinMaxSumLazySegmentTreeLeaf<TValue, TLazy> : ISegmentTreeNode<TValue[], TLazy[]>, ISegmentTreeNodeAttributeIndexable<MinMaxSumLazySegmentTreeLeafAttributeType, MinMaxSumLazySegmentTreeLeafLazyAttributeType>
    {
        // Attributes: 0 = min, 1 = max, 2 = sum, 3 = lazy atributes
        private TValue[] _attributes;
        private readonly int _leftRange, _rightRange;
        private TValue[] _defaultAttributes;

        public MinMaxSumLazySegmentTreeLeaf(TValue[] attributes,
            TValue[] defaultAttributes, int leftRange, int rightRange)
        {
            _attributes = new TValue[attributes.Length];
            Array.Copy(attributes, _attributes, attributes.Length);
            this._leftRange = leftRange;
            this._rightRange = rightRange;
            _defaultAttributes = defaultAttributes;
        }

        public bool IsLeaf()
        {
            return true;
        }

        public (int, int) GetRange()
        {
            return (_leftRange, _rightRange);
        }

        public ref TValue[] GetAttributesRef()
        {
            return ref _attributes;
        }



        public ref TLazy[] GetLazyAttributesRef()
        {
            throw new NotImplementedException();
        }

        public static int GetAttributeIndex(MinMaxSumLazySegmentTreeLeafAttributeType attribute)
        {
            return attribute switch
            {
                MinMaxSumLazySegmentTreeLeafAttributeType.Raw => 0,
                _ => throw new NotImplementedException(),
            };
            ;
        }

        public static int GetLazyAttributeIndex(MinMaxSumLazySegmentTreeLeafLazyAttributeType lazyAttribute)
        {
            throw new NotImplementedException();
        }

        public void ResetLazyAttributes() { }

        public void ResetAttributes()
        {
            Array.Copy(_defaultAttributes, _attributes, _attributes.Length);
        }
    }
}
