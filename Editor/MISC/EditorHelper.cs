using UnityEditor;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST.Editor
{
    internal static class EditorHelper
    {
        internal static bool GetBootScenePath(out string bootScenePath)
        {
            // Cant use SceneManager.sceneCountInBuildSettings because at runtimes returns 1 for first active scene in playmode. 
            // Same for SceneUtility.GetScenePathByBuildIndex(0), returns first active scene path.
            if (EditorBuildSettings.scenes.Length >= 1)
            {
                bootScenePath = SceneUtility.GetScenePathByBuildIndex(0);
                return !string.IsNullOrEmpty(bootScenePath);
            }

            bootScenePath = null;
            return false;
        }
    }
}
