using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    [Serializable]
    public class VerifiableRegionLoadInfo<T> : IEquatable<VerifiableRegionLoadInfo<T>>, ISceneListSettable, IVerifiable<RegionLoadInfo> where T : VerifiableSceneAssetReference
    {
        [SerializeField] private T verifyableSceneAssetReference;
        [SerializeField, Range(0, 100)] private int priority;
        [SerializeField] private bool shouldReload;

        public VerifiableRegionLoadInfo(T verifyableSceneAssetReference, int priority, bool shouldReload)
        {
            this.verifyableSceneAssetReference = verifyableSceneAssetReference;
            this.priority = priority;
            this.shouldReload = shouldReload;
        }

        public void SetSceneList(BaseSceneList sceneList)
        {
            if (verifyableSceneAssetReference == null)
            { return; }

            verifyableSceneAssetReference.SetSceneList(sceneList);
        }

        public bool Verify(out RegionLoadInfo regionLoadInfo, out string errorMessage)
        {
            if (verifyableSceneAssetReference == null)
            {
                regionLoadInfo = null;
                errorMessage = "TODO: write error that the internal 'verifyableSceneAssetReference' is null";
                return false;
            }

            bool result = verifyableSceneAssetReference.Verify(out AssetReference sceneAssetReference, out errorMessage);
            if (result)
            {
                regionLoadInfo = new RegionLoadInfo(sceneAssetReference, priority, shouldReload);
                return true;
            }

            regionLoadInfo = null;
            return false;
        } 

#region IEquatable
        public bool Equals(VerifiableRegionLoadInfo<T> other) => verifyableSceneAssetReference?.Equals(other.verifyableSceneAssetReference) ?? false ;
        public override int GetHashCode() => verifyableSceneAssetReference?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => Equals(obj as VerifiableRegionLoadInfo<T>);
#endregion // IEquatable
    }
}
