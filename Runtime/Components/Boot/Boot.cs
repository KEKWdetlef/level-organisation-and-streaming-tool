using KekwDetlef.LOST;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{

#if UNITY_EDITOR
    private AssetReference injectedSceneAssetReference = null;

    internal void InjectSceneAssetReference(AssetReference sceneAssetReference) => injectedSceneAssetReference = sceneAssetReference;

    private void Editor_InitializeWorld()
    {
        if (injectedSceneAssetReference != null)
        {
            initializeWorld.Editor_Run(injectedSceneAssetReference);
        }
        else
        {
            InitializeWorld();
        }
    }
#endif // UNITY_EDITOR

    [SerializeField] private InitializeWorld initializeWorld;

    private void Start()
    {

#if UNITY_EDITOR
        Editor_InitializeWorld();
#else
        InitializeWorld();
#endif // UNITY_EDITOR

        Viewport.Initialize();
        _ = SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    private void InitializeWorld()
    {
        initializeWorld.Run();
    }
}
