
namespace KekwDetlef.LOST
{
    public class InitializeWorld : ComponentSingle<VerifiableRegionAssetReference<LevelList>, RegionAssetReference, LevelList>
    {

#if UNITY_EDITOR
        public void Editor_Run(RegionAssetReference sceneAssetReference)
        {
            // TODO: make sure AssetReference is valid and a scene

            if (!Editor_OnRun(sceneAssetReference, out string errorMessage))
            {
                throw new System.Exception(errorMessage);
            }
        }

        protected override bool Editor_OnRun(RegionAssetReference sceneAssetReference, out string errorMessage)
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

        protected override void OnRun(RegionAssetReference sceneAssetReference)
        {
            if (sceneAssetReference == null) { return; }

            World.Initialize(sceneAssetReference);
        }
    }
}
