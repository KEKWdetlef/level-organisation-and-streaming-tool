using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public class UnloadRegions : VerifiableComponentMultipleSceneAssetReferences
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(AssetReference[] sceneAssetReferences, out string errorMessage)
        {
            if (World.GetInstance(out World instance))
            {
                instance.UnloadRegions(sceneAssetReferences);
                errorMessage = null;
                return true;
            }

            errorMessage = Helper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(AssetReference[] sceneAssetReferences)
        {
            if (World.GetInstance(out World instance))
            {
                instance.UnloadRegions(sceneAssetReferences);
            }
        }
    }
}
