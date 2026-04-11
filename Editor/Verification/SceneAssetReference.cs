using System;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST.Editor
{
    [Serializable]
    public class SceneAssetReference : AssetReferenceT<SceneAsset>
    {
        public SceneAssetReference(string guid) : base(guid) { }
    }
}
