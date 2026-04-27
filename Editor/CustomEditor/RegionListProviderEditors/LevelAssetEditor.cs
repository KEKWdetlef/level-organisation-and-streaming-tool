using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST.Editor
{
    [CustomEditor(typeof(LevelAsset))]
    public class LevelAssetEditor : RegionListProviderEditor
    {
        protected override void Inject(Scene scene)
        {
            if (serializedObject.targetObject is not LevelAsset levelAsset) { return; }

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                IRegionListProviderSettable<LevelAsset>[] settables = root.GetComponentsInChildren<IRegionListProviderSettable<LevelAsset>>(true);
                foreach (IRegionListProviderSettable<LevelAsset> settable in settables)
                {
                    settable.SetRegionListProvider(levelAsset);
                    EditorUtility.SetDirty((Object)settable);
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
    }
}
