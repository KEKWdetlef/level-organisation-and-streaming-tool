using System;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Free : RegionState
    {
        internal Free(Action<RegionState> onChangeState) : base(onChangeState) { }

        internal override void Load(AssetReference sceneAssetReference, LoadSceneMode loadSceneMode, int priority) => ChangeState(new Loading(OnChangeState, sceneAssetReference, loadSceneMode, priority));
        internal override void Unload() { }
    }
}
