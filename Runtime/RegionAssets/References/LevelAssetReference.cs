using System;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class LevelAssetReference : AssetReferenceT<LevelAsset>
    {
        public LevelAssetReference(string guid) : base(guid) { }
        public static implicit operator RegionAssetReference(LevelAssetReference value) => new RegionAssetReference(value.AssetGUID);
    }
}
