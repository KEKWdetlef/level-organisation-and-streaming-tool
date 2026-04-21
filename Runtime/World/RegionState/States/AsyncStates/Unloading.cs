using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal class Unloading : AsyncRegionState
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
        
        protected override IRegionState OnExecutionFinished()
        {
            Helper.AssertHandleValid(unloadHandle);
            currentHandle.Release();
            unloadHandle.Release();
            return new Free();
        }

        protected override IRegionState OnLoad()
        {
            Helper.AssertHandleValid(unloadHandle);
            currentHandle.Release();
            unloadHandle.Release();
            return new Loading(sceneAssetReference, priority);
        }

        protected override IRegionState OnUnload() => OnExecutionFinished();

        protected override Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority)
        {
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
            return Procedure.Other;
        }

        protected override Procedure GetUnloadProcedure() => Procedure.Default;
    }
}
