using SegmentTreeConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsLeaf()
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int, int) GetRange()
        {
            return (_leftRange, _rightRange);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TValue[] GetAttributesRef()
        {
            return ref _attributes;
        }



        public ref TLazy[] GetLazyAttributesRef()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetAttributes()
        {
            Array.Copy(_defaultAttributes, _attributes, _attributes.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue[] GetQueryAttributesRef()
        {
            var rawIndex = GetAttributeIndex(MinMaxSumLazySegmentTreeLeafAttributeType.Raw);
            return [_attributes[rawIndex], _attributes[rawIndex], _attributes[rawIndex]];
        }
    }
}
