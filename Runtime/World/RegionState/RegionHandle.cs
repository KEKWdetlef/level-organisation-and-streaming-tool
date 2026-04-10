using System;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class RegionHandle : IEquatable<RegionHandle>, IEquatable<AssetReference>
    {
        private readonly AssetReference sceneAssetReference;
        private RegionState currentState;

        public bool Free => currentState is Free;

        internal RegionHandle(AssetReference sceneAssetReference)
        {
            this.sceneAssetReference = sceneAssetReference;
            currentState = new Free(ChangeState);
        }

        internal void Load(LoadSceneMode loadSceneMode, int priority) => currentState.Load(sceneAssetReference, loadSceneMode, priority);
        internal void Unload() => currentState.Unload();

        private void ChangeState(RegionState newState) => currentState = newState;

#region IEquatable
        public bool Equals(RegionHandle other) => new AssetReferenceGuidComparer().Equals(sceneAssetReference, other.sceneAssetReference);
        public bool Equals(AssetReference other) => new AssetReferenceGuidComparer().Equals(sceneAssetReference, other);
        public override bool Equals(object obj) => Equals(obj as RegionHandle);
        public override int GetHashCode() => new AssetReferenceGuidComparer().GetHashCode(sceneAssetReference);

#endregion // IEquatable
    }
}
