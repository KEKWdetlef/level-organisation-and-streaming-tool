using System;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class RegionAssetReference : AssetReferenceT<RegionAsset>
    {
        public RegionAssetReference(string guid) : base(guid) { }
    }
}
