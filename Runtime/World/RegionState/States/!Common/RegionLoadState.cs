using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal abstract class RegionLoadState : IDisposable
    {
        public void Dispose() => OnDispose();
        protected virtual void OnDispose() { }

        internal abstract Task<RegionLoadState> Execute();
        internal abstract Task<RegionLoadState> Load(AssetReference sceneAssetReference, int priority, bool shouldReload);
        internal abstract Task<RegionLoadState> Unload();

        internal virtual bool IsUnloaded => false;
    }

    internal abstract class SyncRegionLoadState : RegionLoadState
    {
        internal override Task<RegionLoadState> Execute() => Task.FromResult(OnExecute());
        internal override Task<RegionLoadState> Load(AssetReference sceneAssetReference, int priority, bool shouldreload) => Task.FromResult(OnLoad(sceneAssetReference, priority, shouldreload));
        internal override Task<RegionLoadState> Unload() => Task.FromResult(OnUnload());

        protected abstract RegionLoadState OnExecute();
        protected abstract RegionLoadState OnLoad(AssetReference sceneAssetReference, int priority, bool shouldreload);
        protected abstract RegionLoadState OnUnload();
    }
}