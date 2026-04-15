using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST.Editor
{
    internal class Verifier
    {
        internal bool Run()
        {
            var setup = EditorSceneManager.GetSceneManagerSetup();
            string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                if (VerifyScene(openedScene) == false)
                { return false; }
            }

            EditorSceneManager.RestoreSceneManagerSetup(setup);
            return true;
        }

        private bool VerifyScene(Scene scene)
        {
            bool result = true;

            foreach (var root in scene.GetRootGameObjects())
            {
                VerifiableComponent[] components = root.GetComponentsInChildren<VerifiableComponent>(true);
                foreach (VerifiableComponent component in components)
                {
                    if (!component.Editor_Verify())
                    { 
                        result = false; 
                    }

                    EditorUtility.SetDirty(component);
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            return result;
        }
    }
}
