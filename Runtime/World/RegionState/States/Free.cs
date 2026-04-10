using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal class Free : IRegionState
    {
        async Task<IRegionState> IRegionState.Execute() => null;
        async Task<IRegionState> IRegionState.Load(AssetReference sceneAssetReference, int priority) => new Loading(sceneAssetReference, priority);
        async Task<IRegionState> IRegionState.Unload() => null;
    }
}
