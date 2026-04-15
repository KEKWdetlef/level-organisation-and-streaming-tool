using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class VerifiableSceneAssetReference : IEquatable<VerifiableSceneAssetReference>, ISceneListSettable, IVerifiable<AssetReference>
    {
        public const string SceneListProperty = nameof(sceneList);
        public const string SceneAssetReferenceProperty = nameof(raw);

        [SerializeField] private BaseSceneList sceneList;
        [SerializeField] private AssetReference raw = null;

        public VerifiableSceneAssetReference(BaseSceneList sceneList, AssetReference assetReference)
        {
            this.sceneList = sceneList;
            raw = assetReference;
        }

        public void SetSceneList(BaseSceneList sceneList)
        {
            this.sceneList = sceneList;
        }

        public bool Verify(out AssetReference sceneAssetReference, out string errorMessage)
        {
            if (sceneList == null)
            {
                sceneAssetReference = null;
                errorMessage = "TODO: write error message for missing scene list";
                return false;
            }

            if (raw == null)
            { 
                sceneAssetReference = null;
                errorMessage = "TODO: write error message for missing assetreference";
                return false; 
            }

            bool result = sceneList.IsReferenceValid(raw);
            if (!result)
            {
                sceneAssetReference = null;
                errorMessage = "TODO: write error message for verification failing";
                return false;
            }

            sceneAssetReference = raw;
            errorMessage = null;
            return result;
        }
        
#region IEquatable
        public override int GetHashCode() => new AssetReferenceGuidComparer().GetHashCode(raw);
        public override bool Equals(object obj) => Equals(obj as VerifiableSceneAssetReference);
        public bool Equals(VerifiableSceneAssetReference other) => new AssetReferenceGuidComparer().Equals(raw, other.raw);
#endregion // IEquatable
    }
}
