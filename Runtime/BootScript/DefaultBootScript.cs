using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    public class DefaultBootScript : BootScript
    {

#if UNITY_EDITOR
        protected override void Editor_OnRun(RegionAssetReference sceneAssetReference)
        {
            if (sceneAssetReference == null)
            {
                initializeWorld.Run();
            }
            else
            {
                initializeWorld.Editor_Run(sceneAssetReference);
            }

            Common();
        }
#else
        protected override void Run()
        {
            initializeWorld.Run();
            Common();
        }
#endif // UNITY_EDITOR

        private void Common()
        {
            Viewport.Initialize();
            _ = SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}
