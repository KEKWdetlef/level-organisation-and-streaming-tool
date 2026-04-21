using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public class InitializeWorld : VerifiableComponentSingleSceneAssetReference
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(AssetReference sceneAssetReference, out string errorMessage)
        {
            if (WorldState.Initialize(gameObject.scene, sceneAssetReference))
            {
                errorMessage = null;
                return true;
            }

            errorMessage = "TODO: write error message, that the worldstate is already initialized and can not be initialized again";
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(AssetReference sceneAssetReference)
        {
            if (sceneAssetReference == null) { return; }

            WorldState.Initialize(gameObject.scene, sceneAssetReference);
        }
    }
}
