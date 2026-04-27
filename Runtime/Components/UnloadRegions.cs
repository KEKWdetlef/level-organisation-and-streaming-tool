
namespace KekwDetlef.LOST
{
    public class UnloadRegions : ComponentMultiple<VerifiableRegionAssetReference<LevelAsset>, RegionAssetReference, LevelAsset>
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(RegionAssetReference[] sceneAssetReferences, out string errorMessage)
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

        protected override void OnRun(RegionAssetReference[] sceneAssetReferences)
        {
            if (World.GetInstance(out World instance))
            {
                instance.UnloadRegions(sceneAssetReferences);
            }
        }
    }
}
