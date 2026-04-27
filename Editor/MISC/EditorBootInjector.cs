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

            if (!EditorHelper.GetBootScenePath(out string bootScenePath))
            {
                Clear();
                Debug.LogError("TODO: write error message for no valid boot scene set.");
                return;
            }

            if (!scene.path.Equals(bootScenePath)) 
            { 
                Clear();
                return; 
            }

            List<BootScript> bootScripts = new List<BootScript>();
            foreach (var root in scene.GetRootGameObjects())
            {
                bootScripts.AddRange(root.GetComponentsInChildren<BootScript>(true));
            }

            if (bootScripts.Count != 1)
            {
                // TODO: maybe exit playmode
                Debug.LogError("TODO: write error message for invalid amound of bootscripts");
                Clear();
                return;
            }

            BootScript bootScript = bootScripts[0];

            RegionAssetReference sceneAssetReference = Consume();
            bootScript.Editor_Run(sceneAssetReference);
        }

        internal static void Set(RegionAssetReference sceneAssetReference)
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

        private static RegionAssetReference Consume()
        {
            string guid = SessionState.GetString(SceneGuidKey, string.Empty);

            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            SessionState.EraseString(SceneGuidKey);
            return new RegionAssetReference(guid);
        }
    }
}
