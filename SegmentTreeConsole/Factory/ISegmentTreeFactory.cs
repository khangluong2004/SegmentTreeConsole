namespace SegmentTreeConsole.Factory
{
    public interface ISegmentTreeFactory<TValue, TValueUpdate, TLazy>
    {
        public GenericSegmentTree<TValue, TValueUpdate, TLazy> CreateSegmentTree(TValue[] data);
    }
}
