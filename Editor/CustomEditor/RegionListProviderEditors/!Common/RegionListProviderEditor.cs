using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

namespace KekwDetlef.LOST.Editor
{
    public abstract class RegionListProviderEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            Button applyButton = new Button(Apply)
            {
                text = "Apply",
            };
            root.Add(applyButton);

            return root;
        }

        private void Apply()
        {
            if (serializedObject.targetObject is not IRegionListProvider regionListProvider)
            {
                Debug.LogError($"Can not apply, as the object itself is not a {nameof(IRegionListProvider)}");
                return;
            }

            RegionAssetReference[] regionAssetReferences = regionListProvider.Editor_RegionAssetReferences;
            string[] guids = GetGuids(regionAssetReferences);

            // TODO: starting here stuff might get unsafe

            SceneSetup[] setup = EditorSceneManager.GetSceneManagerSetup();
            foreach (string guid in guids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                Scene openScene = EditorSceneManager.OpenScene(scenePath);

                Inject(openScene);
            }

            EditorSceneManager.RestoreSceneManagerSetup(setup);
        }

        protected abstract void Inject(Scene scene);

        private string[] GetGuids(RegionAssetReference[] regionAssetReferences)
        {
            int length = regionAssetReferences.Length;
            string[] result = new string[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = regionAssetReferences[i].editorAsset.SceneAssetGuid.Value;
            }

            return result;
        }
    }
}
