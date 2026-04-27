using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class SceneAssetGuid
    {

#if UNITY_EDITOR
        public const string Editor_SceneAssetReferenceProperty = nameof(sceneAssetReference);
        public const string Editor_ValueProperty = nameof(value);

        [SerializeField] private AssetReferenceT<UnityEditor.SceneAsset> sceneAssetReference;
#endif // UNITY_EDITOR

        [SerializeField] private string value;
        public string Value => value;

        private SceneAssetGuid() { }
    }
}
