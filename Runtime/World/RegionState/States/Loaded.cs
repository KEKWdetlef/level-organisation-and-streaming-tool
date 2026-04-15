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
        
        protected override IRegionState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldReload)
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

        protected override IRegionState OnUnload() => new Unloading(handle);
    }
}
