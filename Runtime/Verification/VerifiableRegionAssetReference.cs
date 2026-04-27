using System;
using UnityEngine;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class VerifiableRegionAssetReference<TProvider> : IEquatable<VerifiableRegionAssetReference<TProvider>>
                                                           , IRegionListProviderSettable<TProvider> 
                                                           , IVerifiable<RegionAssetReference> 
    where TProvider : ScriptableObject, IRegionListProvider
    {
        public const string RegionListProviderProperty = nameof(regionListProvider);
        public const string RegionAssetReferenceProperty = nameof(raw);

        [SerializeField] private TProvider regionListProvider = default;
        [SerializeField] private RegionAssetReference raw = null;

        private VerifiableRegionAssetReference() { }

        public void SetRegionListProvider(TProvider regionListProvider)
        {
            this.regionListProvider = regionListProvider;
        }

        public bool Verify(out RegionAssetReference regionAssetReference, out string errorMessage)
        {
            if (this.regionListProvider == null)
            {
                regionAssetReference = null;
                errorMessage = "TODO: write error message for missing scene list";
                return false;
            }

            if (this.regionListProvider is not IRegionListProvider regionListProvider)
            {
                regionAssetReference = null;
                errorMessage = "TODO: write error message for cached object is somehow not a IRegionListProvider";
                return false;
            }

            if (raw == null)
            { 
                regionAssetReference = null;
                errorMessage = "TODO: write error message for missing assetreference";
                return false; 
            }

            bool result = regionListProvider.Editor_Contains(raw);
            if (!result)
            {
                regionAssetReference = null;
                errorMessage = "TODO: write error message for verification failing";
                return false;
            }

            regionAssetReference = raw;
            errorMessage = null;
            return result;
        }
        
#region IEquatable
        public override int GetHashCode() => new AssetReferenceGuidComparer().GetHashCode(raw);
        public override bool Equals(object obj) => Equals(obj as VerifiableRegionAssetReference<TProvider>);
        public bool Equals(VerifiableRegionAssetReference<TProvider> other) => new AssetReferenceGuidComparer().Equals(raw, other.raw);

        #endregion // IEquatable
    }
}
