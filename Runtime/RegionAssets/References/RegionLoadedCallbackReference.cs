using System;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class RegionLoadedCallbackReference : AssetReferenceT<RegionLoadedCallback>
    {
        public RegionLoadedCallbackReference(string guid) : base(guid) { }
    }
}
