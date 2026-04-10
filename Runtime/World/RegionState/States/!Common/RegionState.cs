
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal abstract class RegionState
    {
        private readonly Action<RegionState> onChangeState;
        protected Action<RegionState> OnChangeState => onChangeState;

        internal RegionState(Action<RegionState> onChangeState)
        {
            this.onChangeState = onChangeState;
        }

        internal abstract void Load(AssetReference sceneAssetReference, int priority);
        internal abstract void Unload();

        protected void ChangeState(RegionState newState)
        {
            onChangeState?.Invoke(newState);
        }
    }
}
