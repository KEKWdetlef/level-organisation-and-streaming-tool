using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public class LoadLevel : VerifiableComponentSingleSceneAssetReference
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(AssetReference sceneAssetReference, out string errorMessage)
        {
            if (World.GetInstance(out World instance))
            {
                instance.LoadLevel(sceneAssetReference);
                errorMessage = null;
                return true;
            }

            errorMessage = Helper.InvalidWorldStateErrorMessage;               
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(AssetReference sceneAssetReference)
        {
            if (World.GetInstance(out World instance))
            {
                instance.LoadLevel(sceneAssetReference);
            }
        }
    }
}
