using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public abstract class BaseSceneList : ScriptableObject
    {
        public abstract AssetReference[] GetSceneReferences();
        public abstract bool IsReferenceValid(AssetReference assetReference);
    }
}
