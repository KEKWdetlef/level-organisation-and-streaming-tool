using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Unloading : AsyncRegionLoadState
    {
        private readonly AsyncOperationHandle currentHandle;
        private AsyncOperationHandle unloadHandle;

        private AssetReference sceneAssetReference;
        private int priority;

        internal Unloading(AsyncOperationHandle currentHandle)
        {
            this.currentHandle = currentHandle;
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
            return new Unloaded();
        }

        protected override RegionLoadState OnLoad()
        {
            Helper.AssertHandleValid(unloadHandle);
            currentHandle.Release();
            unloadHandle.Release();
            return new Loading(sceneAssetReference, priority);
        }

        protected override RegionLoadState OnUnload() => OnExecutionFinished();

        protected override Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority)
        {
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
            return Procedure.Other;
        }

        protected override Procedure GetUnloadProcedure() => Procedure.Default;
    }
}
