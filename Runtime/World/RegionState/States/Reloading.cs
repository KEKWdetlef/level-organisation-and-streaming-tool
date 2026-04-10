using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Reloading : RegionState
    {
        private readonly AssetReference sceneAssetReference;
        private int priority;

        private ProceedWith proceedWith;

        internal Reloading(Action<RegionState> onChangeState, AsyncOperationHandle currentHandle, AssetReference sceneAssetReference, int priority) : base(onChangeState)
        {
            proceedWith = ProceedWith.Load;

            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;

            AsyncOperationHandle newHandle = Addressables.UnloadSceneAsync(currentHandle, autoReleaseHandle: true);
            newHandle.Completed += Proceede;

            currentHandle.Release();
        }

        private void Proceede(AsyncOperationHandle handle)
        {
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                // TODO: error display or crash
            }

            switch(proceedWith)
            {
                case ProceedWith.Load: { ChangeState(new Loading(OnChangeState, sceneAssetReference, priority)); } break;
                case ProceedWith.Unload: { ChangeState(new Free(OnChangeState)); } break;
            }
        }

        internal override void Load(AssetReference sceneAssetReference, int priority)
        {
            this.priority = priority;
            proceedWith = ProceedWith.Load;
        }

        internal override void Unload() => proceedWith = ProceedWith.Unload;
    }
}
