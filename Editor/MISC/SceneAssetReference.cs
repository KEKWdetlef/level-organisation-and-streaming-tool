
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class SceneAssetReference : AssetReferenceT<SceneAsset>
    {
        public SceneAssetReference(string guid) : base(guid) { }
    }
}

#endif // UNITY_EDITOR