using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Loaded : SyncRegionLoadState
    {
        private readonly AsyncOperationHandle handle;

        internal Loaded(AsyncOperationHandle handle)
        {
            this.handle = handle;
        }

        protected override RegionLoadState OnExecute() => null;
        
        protected override RegionLoadState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldReload)
        {
            if (shouldReload)
            {
                return new Reloading(handle, sceneAssetReference, priority);
            }
            else
            {
                return null;
            }
        }

        protected override RegionLoadState OnUnload() => new Unloading(handle);
    }
}
