using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [CreateAssetMenu(fileName = "RegionAsset", menuName = "LOST/RegionAsset")]
    public class RegionAsset : ScriptableObject
    {
        [SerializeField] private SceneAssetGuid sceneAssetGuid = null;
        [SerializeField] private RegionLoadedCallbackReference callback = null;

        public SceneAssetGuid SceneAssetGuid => sceneAssetGuid;
        public AssetReferenceT<RegionLoadedCallback> RegionloadedCallback => callback;
    }
}
