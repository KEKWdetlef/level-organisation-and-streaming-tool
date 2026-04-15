using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class Free : RegionState
    {
        protected override IRegionState OnExecute() => null;
        protected override IRegionState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldReload) => new Loading(sceneAssetReference, priority);
        protected override IRegionState OnUnload() => null;
    }
}
