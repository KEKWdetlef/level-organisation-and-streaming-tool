using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal interface IRegionState
    {
        internal Task<IRegionState> Execute();
        internal Task<IRegionState> Load(AssetReference sceneAssetReference, int priority);
        internal Task<IRegionState> Unload();
    }
}