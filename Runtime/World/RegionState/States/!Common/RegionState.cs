using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal interface IRegionState : IDisposable
    {
        internal Task<IRegionState> Execute();
        internal Task<IRegionState> Load(AssetReference sceneAssetReference, int priority, bool shouldReload);
        internal Task<IRegionState> Unload();
    }

    internal abstract class RegionState : IRegionState
    {
        void IDisposable.Dispose() { }

        // TODO: make these not async if possile
        Task<IRegionState> IRegionState.Execute() => Task.FromResult(OnExecute());
        Task<IRegionState> IRegionState.Load(AssetReference sceneAssetReference, int priority, bool shouldreload) => Task.FromResult(OnLoad(sceneAssetReference, priority, shouldreload));
        Task<IRegionState> IRegionState.Unload() => Task.FromResult(OnUnload());

        protected abstract IRegionState OnExecute();
        protected abstract IRegionState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldreload);
        protected abstract IRegionState OnUnload();
    }
}