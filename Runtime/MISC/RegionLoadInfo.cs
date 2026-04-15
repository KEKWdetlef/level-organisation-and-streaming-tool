using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class RegionLoadInfo
    {
        [SerializeField] private AssetReference sceneAssetReference;
        [SerializeField, Range(0, 100)] private int priority;
        [SerializeField] private bool shouldReload;

        public AssetReference SceneAssetReference => sceneAssetReference;
        public int Priority => priority;
        public bool ShouldReload => shouldReload;

        public RegionLoadInfo(AssetReference sceneAssetReference, int priority, bool shouldReload)
        {
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
            this.shouldReload = shouldReload;
        }
    }
}
