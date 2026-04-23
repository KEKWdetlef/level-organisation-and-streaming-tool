using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Loading : AsyncRegionLoadState
    {
        private readonly AssetReference sceneAssetReference;
        private readonly int priority;

        AsyncOperationHandle handle = default;

        internal Loading(AssetReference sceneAssetReference, int priority)
        {
            this.sceneAssetReference = sceneAssetReference;
            this.priority = priority;
        }

        protected override Task OnExecute()
        {
            handle = Addressables.LoadSceneAsync(
                sceneAssetReference,
                LoadSceneMode.Additive,
                activateOnLoad: true,
                priority,
                SceneReleaseMode.ReleaseSceneWhenSceneUnloaded
            );

            return handle.Task;
        }

        protected override RegionLoadState OnExecutionFinished()
        {
            Helper.AssertHandleValid(handle);
            return new Loaded(handle);
        }

        protected override RegionLoadState OnLoad() => OnExecutionFinished();

        protected override RegionLoadState OnUnload()
        {
            Helper.AssertHandleValid(handle);
            return new Unloading(handle);
        }

        protected override Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority) => Procedure.Default;
        protected override Procedure GetUnloadProcedure() => Procedure.Other;
    }
}
