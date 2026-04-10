using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Loading : RegionState
    {
        private ProceedWith proceedWith;

        internal Loading(Action<RegionState> onChangeState, AssetReference sceneAssetReference, LoadSceneMode loadSceneMode, int priority) : base(onChangeState)
        {
            proceedWith = ProceedWith.Load;
            
            AsyncOperationHandle handle = Addressables.LoadSceneAsync(
                sceneAssetReference,
                LoadSceneMode.Additive,
                activateOnLoad: true,
                priority,
                SceneReleaseMode.ReleaseSceneWhenSceneUnloaded
            );

            handle.Completed += Procede;
        }

        private void Procede(AsyncOperationHandle handle)
        {
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                // TODO: error display or crash
                
                // ChangeState(new Free(OnChangeState));
                // return;
            }

            switch(proceedWith)
            {
                case ProceedWith.Load: { ChangeState(new Loaded(OnChangeState, handle)); } break;
                case ProceedWith.Unload: { ChangeState(new Unloading(OnChangeState, handle)); } break;
            }
        }

        internal override void Load(AssetReference sceneAssetReference, LoadSceneMode loadSceneMode, int priority) => proceedWith = ProceedWith.Load;
        internal override void Unload() => proceedWith = ProceedWith.Unload;
    }
}
