using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    internal class LevelListWindow : EditorWindow
    {
        [MenuItem("Window/LOST/LevelList")]
        private static void CreateWindow()
        {
            LevelListWindow window = GetWindow<LevelListWindow>();
            window.titleContent = new GUIContent("LevelListWindow");
        }


        [SerializeField] private BaseSceneList levelSceneList = null;

        private void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            VisualElement levelSceneListField = new ObjectField()
            {
                objectType = typeof(BaseSceneList),
                label = "Level Scene List",
            };

            root.Add(levelSceneListField);

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);
        }



        // private SceneSetup[] setup;
        // private void Test()
        // {
        //     if (EditorApplication.isPlaying)
        //     {
        //         Debug.LogError("TODO: Write error that no level can be played while already in playmode");
        //         return;
        //     }

        //     if (!EditorHelper.GetBootScenePath(out string bootScenePath)) { return; }

        //     setup = EditorSceneManager.GetSceneManagerSetup();
            
        //     Scene bootScene = EditorSceneManager.OpenScene(bootScenePath);

        //     // TODO: get the actual sceneAssetReference from the SceneList
        //     AssetReference sceneAssetReference = new AssetReference();
        //     EditorBootInjector.Set(sceneAssetReference);

        //     EditorApplication.EnterPlaymode();
        //     EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        // }

        // private void OnPlayModeStateChanged(PlayModeStateChange change)
        // {
        //     EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            
        //     if (change == PlayModeStateChange.ExitingPlayMode)
        //     {
        //         EditorSceneManager.RestoreSceneManagerSetup(setup);
        //     }
        // }
    }
}
