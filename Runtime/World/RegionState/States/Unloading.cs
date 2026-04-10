using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Unloading : RegionState
    {
        private AssetReference sceneAssetReference;
        private LoadSceneMode loadSceneMode;
        private int priority;

        private ProceedWith proceedWith;

        internal Unloading(Action<RegionState> onChangeState, AsyncOperationHandle currentHandle) : base(onChangeState)
        {
            proceedWith = ProceedWith.Unload;

            AsyncOperationHandle newHandle = Addressables.UnloadSceneAsync(currentHandle, autoReleaseHandle: true);
            newHandle.Completed += Proceede;

            currentHandle.Release();
        }

        private void Proceede(AsyncOperationHandle handle)
        {
            switch(proceedWith)
            {
                case ProceedWith.Load: { ChangeState(new Loading(OnChangeState, sceneAssetReference, loadSceneMode, priority)); } break;
                case ProceedWith.Unload: { ChangeState(new Free(OnChangeState)); } break;
            }
        }

        internal override void Load(AssetReference sceneAssetReference, LoadSceneMode loadSceneMode, int priority)
        {
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
            this.loadSceneMode = loadSceneMode;
            
            proceedWith = ProceedWith.Load;
        }

        internal override void Unload() => proceedWith = ProceedWith.Unload;
    }
}
