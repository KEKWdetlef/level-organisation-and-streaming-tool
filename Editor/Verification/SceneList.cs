using System.Linq;
using KekwDetlef.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST.Editor
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "LOST/SceneList")]
    public class SceneList : BaseSceneList
    {
        // PROBLEM: research what happens if a not addressable asset keeps beeing referenced. 
        [SerializeField] private SCHashSet<SceneAssetReference, AssetReferenceGuidComparer> sceneAssetReferences = new();

        public override AssetReference[] GetSceneReferences() => sceneAssetReferences.ToArray();

        public override bool IsReferenceValid(AssetReference inAssetReference)
        {
            if (inAssetReference == null) { return false; }

            AssetReferenceGuidComparer comparer = new AssetReferenceGuidComparer();
            foreach(AssetReference assetReference in sceneAssetReferences)
            {
                if (comparer.Equals(assetReference, inAssetReference)) { return true; }
            }

            return false;
        }
    }
}
