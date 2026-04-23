using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Reloading : AsyncRegionLoadState
    {
        private readonly AsyncOperationHandle currentHandle;
        private readonly AssetReference sceneAssetReference;

        private int priority;

        private AsyncOperationHandle unloadHandle;

        internal Reloading(AsyncOperationHandle currentHandle, AssetReference sceneAssetReference, int priority)
        {
            this.currentHandle = currentHandle;
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
        }

        protected override Task OnExecute()
        {
            unloadHandle = Addressables.UnloadSceneAsync(currentHandle, autoReleaseHandle: false);
            return unloadHandle.Task;
        }
        
        protected override RegionLoadState OnExecutionFinished()
        {
            Helper.AssertHandleValid(unloadHandle);
            currentHandle.Release();
            unloadHandle.Release();
            return new Loading(sceneAssetReference, priority);
        }

        protected override RegionLoadState OnLoad() => OnExecutionFinished();

        protected override RegionLoadState OnUnload()
        {
            Helper.AssertHandleValid(unloadHandle);
            currentHandle.Release();
            unloadHandle.Release();
            return new Unloaded();
        }

        protected override Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority)
        {
            this.priority = priority;
            return Procedure.Default;
        }

        protected override Procedure GetUnloadProcedure() => Procedure.Other;
    }
}
