using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST.Editor
{
    [InitializeOnLoad]
    internal static class EditorBootInjector
    {
        private const string SceneGuidKey = "EditorBootInjector.SceneGuid";

        static EditorBootInjector()
        {
            Clear();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        internal static void Clear() => SessionState.EraseString(SceneGuidKey);

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    Clear();
                    break;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InjectIntoFirstScene()
        {
            SceneManager.sceneLoaded -= OnFirstSceneLoaded;
            SceneManager.sceneLoaded += OnFirstSceneLoaded;
        }

        private static void OnFirstSceneLoaded(Scene scene, LoadSceneMode _)
        {
            SceneManager.sceneLoaded -= OnFirstSceneLoaded;

            if (!Helper.GetBootScene(out Scene bootScene) || !bootScene.handle.Equals(scene.handle)) 
            { 
                Clear();
                return; 
            }

            List<Boot> bootScripts = new List<Boot>();
            foreach (var root in bootScene.GetRootGameObjects())
            {
                bootScripts.AddRange(root.GetComponentsInChildren<Boot>(true));
            }

            if (bootScripts.Count != 1)
            {
                // TODO: maybe exit playmode
                Debug.LogError("TODO: write error message for invalid amound of bootscripts");
                Clear();
                return;
            }

            Boot bootScript = bootScripts[0];

            if (Consume(out AssetReference sceneAssetReference))
            {
                bootScript.Editor_Run(sceneAssetReference);
            }
        }

        internal static void Set(AssetReference sceneAssetReference)
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogError("TODO: Write error that the boot injection can only be set when in edit mode");
                return;
            }

            if (sceneAssetReference == null || string.IsNullOrWhiteSpace(sceneAssetReference.AssetGUID))
            {
                Clear();
                return;
            }

            SessionState.SetString(SceneGuidKey, sceneAssetReference.AssetGUID);
        }

        private static bool Consume(out AssetReference sceneAssetReference)
        {
            string guid = SessionState.GetString(SceneGuidKey, string.Empty);

            if (string.IsNullOrEmpty(guid))
            {
                sceneAssetReference = null;
                return false;
            }

            SessionState.EraseString(SceneGuidKey);
            sceneAssetReference = new AssetReference(guid);
            return true;
        }
    }
}
