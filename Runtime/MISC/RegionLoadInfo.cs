using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class RegionLoadInfo
    {
        [SerializeField] private RegionAssetReference regionAssetReference;
        [SerializeField, Range(0, 100)] private int priority;
        [SerializeField] private bool shouldReload;

        public AssetReference RegionAssetReference => regionAssetReference;
        public int Priority => priority;
        public bool ShouldReload => shouldReload;

        public RegionLoadInfo(RegionAssetReference regionAssetReference, int priority, bool shouldReload)
        {
            this.regionAssetReference = regionAssetReference;
            this.priority = priority;
            this.shouldReload = shouldReload;
        }
    }
}
