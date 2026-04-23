using System;
using System.Collections.Generic;
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

            Scene bootScene = SceneManager.GetSceneByBuildIndex(0);
            if (!bootScene.IsValid())
            {
                Debug.LogError("TODO: Write error message for invalid bootscene");
                return;
            }

            SceneSetup[] setup = EditorSceneManager.GetSceneManagerSetup();
            
            bootScene = EditorSceneManager.OpenScene(bootScene.path);
            
            List<Boot> bootScripts = new List<Boot>();
            foreach (var root in bootScene.GetRootGameObjects())
            {
                bootScripts.AddRange(root.GetComponentsInChildren<Boot>(true));
            }

            if (bootScripts.Count != 1)
            {
                Debug.LogError("TODO: write error message for invalid amound of bootscripts");
                return;
            }

            Boot bootScript = bootScripts[0];

            // TODO: get the actual sceneAssetReference from the SceneList
            AssetReference sceneAssetReference = new AssetReference();
            bootScript.InjectSceneAssetReference(sceneAssetReference);

            EditorApplication.EnterPlaymode();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            EditorSceneManager.RestoreSceneManagerSetup(setup);
            throw new NotImplementedException();
        }
    }
}
