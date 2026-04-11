using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal interface IRegionState : IDisposable
    {
        internal Task<IRegionState> Execute();
        internal Task<IRegionState> Load(AssetReference sceneAssetReference, int priority);
        internal Task<IRegionState> Unload();
    }

    internal abstract class RegionState : IRegionState
    {
        void IDisposable.Dispose() { }

        // TODO: make these not async if possile
        async Task<IRegionState> IRegionState.Execute() => OnExecute();
        async Task<IRegionState> IRegionState.Load(AssetReference sceneAssetReference, int priority) => OnLoad(sceneAssetReference, priority);
        async Task<IRegionState> IRegionState.Unload() => OnUnload();

        protected abstract IRegionState OnExecute();
        protected abstract IRegionState OnLoad(AssetReference sceneAssetReference, int priority);
        protected abstract IRegionState OnUnload();
    }
}