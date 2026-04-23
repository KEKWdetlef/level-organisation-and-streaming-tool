using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public class InitializeWorld : VerifiableComponentSingleSceneAssetReference
    {

#if UNITY_EDITOR
        internal void Editor_Run(AssetReference sceneAssetReference)
        {
            // TODO: make sure AssetReference is valid and a scene

            if (!Editor_OnRun(sceneAssetReference, out string errorMessage))
            {
                throw new System.Exception(errorMessage);
            }
        }

        protected override bool Editor_OnRun(AssetReference sceneAssetReference, out string errorMessage)
        {
            if (World.Initialize(sceneAssetReference))
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

            World.Initialize(sceneAssetReference);
        }
    }
}
