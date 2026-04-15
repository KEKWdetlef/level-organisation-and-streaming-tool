using System;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class VerifiableSceneReferenceHiddenSceneList : VerifiableSceneAssetReference
    {
        public VerifiableSceneReferenceHiddenSceneList(BaseSceneList sceneList, AssetReference assetReference) : base(sceneList, assetReference) { }
    }
}
