using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    [CustomPropertyDrawer(typeof(VerifiableSceneAssetReference), true)]
    public class VerifiableSceneReferencePropertyDrawer : PropertyDrawer
    {
        protected SerializedProperty propertySceneList = null;
        protected SerializedProperty propertySceneAssetReference = null;

        protected VisualElement root = null;
        protected PropertyField fieldSceneList = null;
        protected AssetReferenceDropdownField ardf = null;
        protected PropertyField fieldSceneAssetReference = null;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();

            propertySceneList = property.FindPropertyRelative(VerifiableSceneAssetReference.SceneListProperty);
            propertySceneAssetReference = property.FindPropertyRelative(VerifiableSceneAssetReference.SceneAssetReferenceProperty);

            fieldSceneList = new PropertyField(propertySceneList, "SceneList");
            fieldSceneList.Bind(property.serializedObject);
            root.Add(fieldSceneList);

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

            fieldSceneList.RegisterValueChangeCallback((evt) => OnSceneListChanged(evt.changedProperty.boxedValue as BaseSceneList));
            ardf.RegisterValueChangedCallback(OnSceneAssetReferenceChosen);

            return root;
        }

        protected void OnSceneAssetReferenceChosen(ChangeEvent<AssetReference> evt)
        {
            if (evt.newValue == null) { return; }

            propertySceneAssetReference.serializedObject.Update();
            propertySceneAssetReference.boxedValue = new AssetReference(evt.newValue.AssetGUID);
            propertySceneAssetReference.serializedObject.ApplyModifiedProperties();
        }

        protected void OnSceneListChanged(BaseSceneList newSceneList)
        {
            GetChoices(newSceneList, out List<AssetReference> sceneAssetReferences, out int index);
            ardf.choices = sceneAssetReferences;
            ardf.index = index;

            if (newSceneList == null)
            {
                if (!root.Contains(ardf)) { return; }
                root.Remove(ardf);
            }
            else
            {
                if (root.Contains(ardf)) { return; }
                root.Add(ardf);
            }
        }

        protected void GetChoices(BaseSceneList sceneList, out List<AssetReference> sceneAssetReferences, out int index)
        {
            index = -1;
            sceneAssetReferences = sceneList == null ? new List<AssetReference>() : sceneList.GetSceneReferences().ToList();

            if (propertySceneAssetReference.boxedValue is AssetReference currentSceneAssetReference)
            {
                index = IndexOf(sceneAssetReferences, currentSceneAssetReference);
            }
        }

        protected int IndexOf(List<AssetReference> sceneAssetReferences, AssetReference currentSceneAssetReference)
        {
            AssetReferenceGuidComparer comparer = new AssetReferenceGuidComparer();

            for (int i = 0; i < sceneAssetReferences.Count; i++)
            {
                AssetReference item = sceneAssetReferences[i];
                
                if (comparer.Equals(item, currentSceneAssetReference))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
