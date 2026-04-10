using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal class Loaded : RegionState
    {
        private AsyncOperationHandle handle;

        internal Loaded(Action<RegionState> onChangeState, AsyncOperationHandle handle) : base(onChangeState)
        {
            this.handle = handle;
        }
        
        internal override void Load(AssetReference sceneAssetReference, LoadSceneMode loadSceneMode, int priority) => ChangeState(new Reloading(OnChangeState, handle, sceneAssetReference, loadSceneMode, priority));
        internal override void Unload() => ChangeState(new Unloading(OnChangeState, handle));
    }
}
