using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST.Editor
{
    [CustomEditor(typeof(LevelList))]
    public class LevelListEditor : RegionListProviderEditor
    {
        protected override void Inject(Scene scene)
        {
            if (serializedObject.targetObject is not LevelList levelList) { return; }

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                IRegionListProviderSettable<LevelList>[] settables = root.GetComponentsInChildren<IRegionListProviderSettable<LevelList>>(true);
                foreach (IRegionListProviderSettable<LevelList> settable in settables)
                {
                    settable.SetRegionListProvider(levelList);
                    EditorUtility.SetDirty((Object)settable);
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
    }
}
