using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public class UnloadRegions : VerifiableComponentMultipleSceneAssetReferences
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(AssetReference[] sceneAssetReferences, out string errorMessage)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.UnloadRegions(sceneAssetReferences);
                errorMessage = null;
                return true;
            }

            errorMessage = LOSTHelper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(AssetReference[] sceneAssetReferences)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.UnloadRegions(sceneAssetReferences);
            }
        }
    }
}
