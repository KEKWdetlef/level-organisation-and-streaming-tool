using System.Linq;
using KekwDetlef.SerializedCollections;
using UnityEngine;

namespace KekwDetlef.LOST
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "LOST/LevelList")]
    public class LevelList : ScriptableObject

#if UNITY_EDITOR
        , IRegionListProvider
#endif // UNITY_EDITOR

    {

#if UNITY_EDITOR
        [SerializeField] private SCHashSet<LevelAssetReference, AssetReferenceGuidComparer> levelAssetReferences = new SCHashSet<LevelAssetReference, AssetReferenceGuidComparer>();

        public RegionAssetReference[] Editor_RegionAssetReferences
        {
            get
            {
                RegionAssetReference[] result = new RegionAssetReference[levelAssetReferences.Count()];
                int index = 0;
                foreach (RegionAssetReference item in levelAssetReferences)
                {
                    result[index] = item;
                }
                
                return result;
            }
        }

        public bool Editor_Contains(RegionAssetReference regionAssetReference)
        {
            AssetReferenceGuidComparer comparer = new AssetReferenceGuidComparer();

            foreach (RegionAssetReference item in levelAssetReferences)
            {
                if (comparer.Equals(item, regionAssetReference)) { return true; }
            }

            return false;
        }
#endif // UNITY_EDITOR

    }
}
