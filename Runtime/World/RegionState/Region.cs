using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class Region : IEquatable<Region>
    {
        private readonly AssetReference sceneAssetReference;
        private RegionLoadState currentRegionLoadState;

        public bool IsUnloaded => currentRegionLoadState.IsUnloaded;

        internal Region(AssetReference sceneAssetReference)
        {
            this.sceneAssetReference = sceneAssetReference;
            currentRegionLoadState = new Unloaded();
        }

        internal async Task Load(int priority, bool shouldReload)
        {
            try
            {
                RegionLoadState newState = await currentRegionLoadState.Load(sceneAssetReference, priority, shouldReload);
                await ChangeState(newState);
            }
            catch(OperationCanceledException) { }
        }

        internal async Task Unload()
        {
            try
            {
                RegionLoadState newState = await currentRegionLoadState.Unload();
                await ChangeState(newState);
            }
            catch(OperationCanceledException) { }
        }

        private async Task ChangeState(RegionLoadState newState)
        {
            while (newState != null)
            {
                currentRegionLoadState.Dispose();
                currentRegionLoadState = newState;
                newState = await currentRegionLoadState.Execute();
            }
        }

#region IEquatable
        public bool Equals(Region other) => new AssetReferenceGuidComparer().Equals(sceneAssetReference, other.sceneAssetReference);
        public override bool Equals(object obj) => Equals(obj as Region);
        public override int GetHashCode() => new AssetReferenceGuidComparer().GetHashCode(sceneAssetReference);
#endregion // IEquatable
    }
}
