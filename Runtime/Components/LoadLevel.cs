
namespace KekwDetlef.LOST
{
    public class LoadLevel : ComponentSingle<VerifiableRegionAssetReference<LevelList>, RegionAssetReference, LevelList> 
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(RegionAssetReference sceneAssetReference, out string errorMessage)
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

        protected override void OnRun(RegionAssetReference sceneAssetReference)
        {
            if (World.GetInstance(out World instance))
            {
                instance.LoadLevel(sceneAssetReference);
            }
        }
    }
}
