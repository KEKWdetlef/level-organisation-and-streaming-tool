using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Loaded : RegionState
    {
        private readonly AsyncOperationHandle handle;

        internal Loaded(AsyncOperationHandle handle)
        {
            this.handle = handle;
        }

        protected override IRegionState OnExecute() => null;
        protected override IRegionState OnLoad(AssetReference sceneAssetReference, int priority) => new Reloading(handle, sceneAssetReference, priority);
        protected override IRegionState OnUnload() => new Unloading(handle);
    }
}
