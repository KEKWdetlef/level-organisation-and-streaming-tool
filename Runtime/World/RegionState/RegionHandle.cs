using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class RegionHandle : IEquatable<RegionHandle>, IEquatable<AssetReference>
    {
        private readonly AssetReference sceneAssetReference;
        private IRegionState currentState;

        public bool IsFree => currentState is Free;

        internal RegionHandle(AssetReference sceneAssetReference)
        {
            this.sceneAssetReference = sceneAssetReference;
            currentState = new Free();
        }

        internal async Task Load(int priority, bool shouldReload)
        {
            try
            {
                IRegionState newState = await currentState.Load(sceneAssetReference, priority, shouldReload);
                await ChangeState(newState);
            }
            catch(OperationCanceledException) { }
        }

        internal async Task Unload()
        {
            try
            {
                IRegionState newState = await currentState.Unload();
                await ChangeState(newState);
            }
            catch(OperationCanceledException) { }
        }

        private async Task ChangeState(IRegionState newState)
        {
            while (newState != null)
            {
                currentState = newState;
                newState = await currentState.Execute();
            }
        }

#region IEquatable
        public bool Equals(RegionHandle other) => new AssetReferenceGuidComparer().Equals(sceneAssetReference, other.sceneAssetReference);
        public bool Equals(AssetReference other) => new AssetReferenceGuidComparer().Equals(sceneAssetReference, other);
        public override bool Equals(object obj) => Equals(obj as RegionHandle);
        public override int GetHashCode() => new AssetReferenceGuidComparer().GetHashCode(sceneAssetReference);
#endregion // IEquatable
    }
}
