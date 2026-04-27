using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class VerifiableRegionLoadInfo<TProvider> : IEquatable<VerifiableRegionLoadInfo<TProvider>>
                                                     , IRegionListProviderSettable<TProvider>
                                                     , IVerifiable<RegionLoadInfo>
    where TProvider : ScriptableObject, IRegionListProvider
    {
        [SerializeField] private VerifiableRegionAssetReference<TProvider> verifyableRegionAssetReference;
        [SerializeField, Range(0, 100)] private int priority;
        [SerializeField] private bool shouldReload;

        private VerifiableRegionLoadInfo() { }

        public void SetRegionListProvider(TProvider regionListProvider) => verifyableRegionAssetReference?.SetRegionListProvider(regionListProvider);

        public bool Verify(out RegionLoadInfo regionLoadInfo, out string errorMessage)
        {
            if (verifyableRegionAssetReference == null)
            {
                regionLoadInfo = null;
                errorMessage = "TODO: write error that the internal 'verifyableSceneAssetReference' is null";
                return false;
            }

            bool result = verifyableRegionAssetReference.Verify(out RegionAssetReference regionAssetReference, out errorMessage);
            if (result)
            {
                regionLoadInfo = new RegionLoadInfo(regionAssetReference, priority, shouldReload);
                return true;
            }

            regionLoadInfo = null;
            return false;
        } 

#region IEquatable
        public bool Equals(VerifiableRegionLoadInfo<TProvider> other) => verifyableRegionAssetReference?.Equals(other.verifyableRegionAssetReference) ?? false ;
        public override int GetHashCode() => verifyableRegionAssetReference?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => Equals(obj as VerifiableRegionLoadInfo<TProvider>);
#endregion // IEquatable
    }
}
