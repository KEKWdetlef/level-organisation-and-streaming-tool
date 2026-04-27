
namespace KekwDetlef.LOST
{

#if UNITY_EDITOR
    public interface IRegionListProvider
    {
        public RegionAssetReference[] Editor_RegionAssetReferences { get; }
        public bool Editor_Contains(RegionAssetReference regionAssetReference);
    }
#endif // UNITY_EDITOR

}
