using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public sealed class AssetReferenceGuidComparer : IEqualityComparer<AssetReference>
    {
        public bool Equals(AssetReference a, AssetReference b)
        {
            if (a == null || b == null)
                return false;

            if (ReferenceEquals(a, b))
                return true;

            return a.AssetGUID == b.AssetGUID;
        }

        public int GetHashCode(AssetReference assetReference) => assetReference?.AssetGUID?.GetHashCode() ?? 0;
    }
}
