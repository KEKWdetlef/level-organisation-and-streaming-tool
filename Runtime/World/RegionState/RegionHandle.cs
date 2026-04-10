using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class RegionHandle : IEquatable<RegionHandle>, IEquatable<AssetReference>
    {
        private readonly AssetReference sceneAssetReference;
        private IRegionState currentState;

        public bool Free => currentState is Free;

        internal RegionHandle(AssetReference sceneAssetReference)
        {
            this.sceneAssetReference = sceneAssetReference;
            currentState = new Free();
        }

        internal async Task Load(int priority)
        {
            IRegionState newState = await currentState.Load(sceneAssetReference, priority);
            await ChangeState(newState);
        }

        internal async Task Unload()
        {
            IRegionState newState = await currentState.Unload();
            await ChangeState(newState);
        }


        // PROBLEM: Make this not be recursive
        // private async Task ChangeState(IRegionState newState)
        // {
        //     while (newState != null)
        //     {
        //         currentState = newState;
        //         newState = await currentState.Execute();
        //     }
        // }
        private async Task ChangeState(IRegionState newState)
        {
            if (newState != null)
            {
                currentState = newState;
                await ChangeState(await newState.Execute());
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
