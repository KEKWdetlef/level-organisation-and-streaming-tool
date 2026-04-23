using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class Unloaded : SyncRegionLoadState
    {
        protected override RegionLoadState OnExecute() => null;
        protected override RegionLoadState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldReload) => new Loading(sceneAssetReference, priority);
        protected override RegionLoadState OnUnload() => null;

        internal override bool IsUnloaded => true;
    }
}
