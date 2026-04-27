using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        private const string Slot = "LevelListWindow_LevelSceneList";
        [SerializeField] private LevelList levelSceneList = null;

        private void OnEnable()
        {
            if (EditorPrefs.HasKey(Slot))
            {
                string jsonString = EditorPrefs.GetString(Slot);
            }
        }

        private void OnDisable()
        {
            
        }

        ListView listView;

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            ObjectField levelSceneListField = new ObjectField()
            {
                objectType = typeof(LevelList),
                label = "Level Scene List",
            };
            root.Add(levelSceneListField);

            listView = new ListView();
            root.Add(listView);

            // levelSceneListField.RegisterValueChangedCallback((evt) => OnSceneListChanged(evt.newValue as BaseLevelList));
        }

        // private void OnSceneListChanged(BaseLevelList newSceneList) 
        // {
        //     levelSceneList = newSceneList;

        //     if (levelSceneList == null) { return; }

        //     BaseLevelAsset[] sceneAssetReferences = levelSceneList.LevelAssets;
        //     if (sceneAssetReferences == null) { return; }

        //     listView.makeItem = () => new Label();
        //     listView.bindItem = (item, index) =>
        //     {
        //         (item as Label).text = sceneAssetReferences[index].editorAsset.name;
        //     };
        //     listView.itemsSource = sceneAssetReferences;
        // }



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
