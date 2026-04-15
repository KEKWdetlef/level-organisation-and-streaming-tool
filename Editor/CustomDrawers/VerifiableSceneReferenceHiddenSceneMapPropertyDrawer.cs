using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    [CustomPropertyDrawer(typeof(VerifiableSceneReferenceHiddenSceneList), true)]
    public class VerifiableSceneReferenceHiddenSceneMapPropertyDrawer : VerifiableSceneReferencePropertyDrawer
    {
        private BaseSceneList currentSceneList = null;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();

            propertySceneList = property.FindPropertyRelative(VerifiableSceneAssetReference.SceneListProperty);
            propertySceneAssetReference = property.FindPropertyRelative(VerifiableSceneAssetReference.SceneAssetReferenceProperty);

            fieldSceneAssetReference = new PropertyField(propertySceneAssetReference, "SceneAssetReference");
            fieldSceneAssetReference.Bind(property.serializedObject);
            fieldSceneAssetReference.SetEnabled(false);
            root.Add(fieldSceneAssetReference);

            BaseSceneList sceneList = propertySceneList.boxedValue as BaseSceneList;
            GetChoices(sceneList, out List<AssetReference> sceneAssetReferences, out int index);
            ardf = new AssetReferenceDropdownField("Scene", sceneAssetReferences, index);

            if (sceneList != null)
            {
                root.Add(ardf);
            }

            ardf.RegisterValueChangedCallback(OnSceneAssetReferenceChosen);
            
            currentSceneList = propertySceneList.boxedValue as BaseSceneList;
            root.schedule.Execute(() => UpdateSceneList(property)).Every(500);

            return root;
        }

        private void UpdateSceneList(SerializedProperty property)
        {
            property.serializedObject.Update();
            
            BaseSceneList newSceneList = propertySceneList.boxedValue as BaseSceneList;
            if (newSceneList == currentSceneList || newSceneList == null) { return; }

            currentSceneList = newSceneList;
            OnSceneListChanged(newSceneList);
        }
    }
}
