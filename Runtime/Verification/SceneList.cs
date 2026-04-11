using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal abstract class SceneList : ScriptableObject
    {
        internal abstract AssetReference[] GetSceneReferences();
        internal abstract bool IsReferenceValid(AssetReference assetReference);
    }
}
