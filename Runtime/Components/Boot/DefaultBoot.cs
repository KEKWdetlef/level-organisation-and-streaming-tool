using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    public class DefaultBoot : Boot
    {

#if UNITY_EDITOR
        internal override void Editor_Run(AssetReference sceneAssetReference)
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
