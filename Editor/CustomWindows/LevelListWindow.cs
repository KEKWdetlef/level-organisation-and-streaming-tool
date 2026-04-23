using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    internal class LevelListWindow : EditorWindow
    {
        private SceneSetup[] setup;

        [MenuItem("Window/LOST/LevelList")]
        private static void CreateWindow()
        {
            LevelListWindow window = GetWindow<LevelListWindow>();
            window.titleContent = new GUIContent("LevelListWindow");
        }

        private void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);
        }

        private void Test()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogError("TODO: Write error that no level can be played while already in playmode");
                return;
            }

            if (!Helper.GetBootScene(out Scene bootScene)) { return; }

            setup = EditorSceneManager.GetSceneManagerSetup();
            
            bootScene = EditorSceneManager.OpenScene(bootScene.path);

            // TODO: get the actual sceneAssetReference from the SceneList
            AssetReference sceneAssetReference = new AssetReference();
            EditorBootInjector.Set(sceneAssetReference);

            EditorApplication.EnterPlaymode();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            
            if (change == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.RestoreSceneManagerSetup(setup);
            }
        }
    }
}
