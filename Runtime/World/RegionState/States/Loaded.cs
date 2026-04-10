using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Loaded : IRegionState
    {
        private AsyncOperationHandle handle;

        internal Loaded(AsyncOperationHandle handle)
        {
            this.handle = handle;
        }

        async Task<IRegionState> IRegionState.Execute() => null;
        async Task<IRegionState> IRegionState.Load(AssetReference sceneAssetReference, int priority) => new Reloading(handle, sceneAssetReference, priority);
        async Task<IRegionState> IRegionState.Unload() => new Unloading(handle);
    }
}
