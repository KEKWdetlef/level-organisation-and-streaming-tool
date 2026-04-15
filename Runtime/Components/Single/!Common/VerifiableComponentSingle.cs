using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public abstract class VerifiableComponentSingle<TEditor, TRuntime> : VerifiableComponent where TEditor : IVerifiable<TRuntime>
    {
        
#if UNITY_EDITOR
        [SerializeField] private TEditor verifiable = default;
        
        protected sealed override bool Editor_OnVerify(out string errorMessage)
        {
            if (verifiable.Verify(out TRuntime runtimeData, out errorMessage))
            {
                this.runtimeData = runtimeData;
                return true;
            }

            return false;
        }

        protected sealed override bool Editor_OnRun(out string errorMessage)
        {
            if (verifiable.Verify(out TRuntime runtimeData, out errorMessage))
            {
                return Editor_OnRun(runtimeData, out errorMessage);
            }
            
            return false;
        }

        protected abstract bool Editor_OnRun(TRuntime runtimeData, out string errorMessage);
#endif // UNITY_EDITOR

        [SerializeField, HideInInspector] private TRuntime runtimeData = default;

        protected sealed override void OnRun() => OnRun(runtimeData);
        protected abstract void OnRun(TRuntime runtimeData);
    }


    public abstract class VerifiableComponentSingleSceneAssetReference : VerifiableComponentSingle<VerifiableSceneAssetReference, AssetReference> { }
    public abstract class VerifiableComponentSingleRegionLoadInfo : VerifiableComponentSingle<VerifiableRegionLoadInfo<VerifiableSceneAssetReference>, RegionLoadInfo> { }
}
