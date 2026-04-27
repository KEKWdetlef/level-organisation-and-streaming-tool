using System.Linq;
using KekwDetlef.SerializedCollections;
using UnityEngine;

namespace KekwDetlef.LOST
{
    [CreateAssetMenu(fileName = "LevelAsset", menuName = "LOST/LevelAsset")]
    public class LevelAsset : RegionAsset

#if UNITY_EDITOR
        , IRegionListProvider
#endif // UNITY_EDITOR
    {

#if UNITY_EDITOR
        [SerializeField] private SCHashSet<RegionAssetReference, AssetReferenceGuidComparer> regionAssetReferences = new SCHashSet<RegionAssetReference, AssetReferenceGuidComparer>();

        public RegionAssetReference[] Editor_RegionAssetReferences => regionAssetReferences.ToArray();
        public bool Editor_Contains(RegionAssetReference regionAssetReference) => regionAssetReferences.Contains(regionAssetReference);
#endif // UNITY_EDITOR

    }
}
