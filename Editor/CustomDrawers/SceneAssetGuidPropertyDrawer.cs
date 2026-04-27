using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    [CustomPropertyDrawer(typeof(SceneAssetGuid), true)]
    public class SceneAssetGuidPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty valueProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty sceneAssetReferenceProperty = property.FindPropertyRelative(SceneAssetGuid.Editor_SceneAssetReferenceProperty);
            valueProperty = property.FindPropertyRelative(SceneAssetGuid.Editor_ValueProperty);

            PropertyField sceneAssetReferenceField = new PropertyField(sceneAssetReferenceProperty);
            root.Add(sceneAssetReferenceField);

            PropertyField valueField = new PropertyField(valueProperty, "GUID")
            {
                enabledSelf = false,
            };
            root.Add(valueField);

            root.TrackPropertyValue(sceneAssetReferenceProperty, OnSceneAssetReferenceChanged);
            return root;
        }

        private void OnSceneAssetReferenceChanged(SerializedProperty property)
        {
            if (property.boxedValue != null && property.boxedValue is AssetReferenceT<SceneAsset> sceneAssetReference)
            {
                valueProperty.serializedObject.Update();
                valueProperty.boxedValue = sceneAssetReference.AssetGUID;
                valueProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
