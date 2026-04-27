using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    [CustomPropertyDrawer(typeof(VerifiableRegionAssetReference<>), true)]
    public class VerifiableSceneReferencePropertyDrawer : PropertyDrawer
    {
        protected SerializedProperty regionListProviderProperty = null;
        protected SerializedProperty regionAssetReferenceProperty = null;

        protected VisualElement root = null;
        protected AssetReferenceDropdownField dropdownField = null;
        protected PropertyField fieldSceneAssetReference = null;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();

            regionListProviderProperty = property.FindPropertyRelative(VerifiableRegionAssetReference<LevelAsset>.RegionListProviderProperty);
            regionAssetReferenceProperty = property.FindPropertyRelative(VerifiableRegionAssetReference<LevelAsset>.RegionAssetReferenceProperty);

            fieldSceneAssetReference = new PropertyField(regionAssetReferenceProperty, "RegionAsset");
            fieldSceneAssetReference.SetEnabled(false);
            root.Add(fieldSceneAssetReference);

            IRegionListProvider regionListProvider = regionListProviderProperty.boxedValue as IRegionListProvider;
            GetChoices(regionListProvider, out List<AssetReference> sceneAssetReferences, out int index);
            dropdownField = new AssetReferenceDropdownField("Scene", sceneAssetReferences, index);

            if (regionListProvider != null)
            {
                root.Add(dropdownField);
            }

            root.TrackPropertyValue(regionListProviderProperty, OnRegionListProviderChanged);
            dropdownField.RegisterValueChangedCallback(OnRegionAssetReferenceChosen);

            return root;
        }

        protected void OnRegionAssetReferenceChosen(ChangeEvent<AssetReference> evt)
        {
            if (evt.newValue == null) { return; }

            regionAssetReferenceProperty.serializedObject.Update();
            regionAssetReferenceProperty.boxedValue = new RegionAssetReference(evt.newValue.AssetGUID);
            regionAssetReferenceProperty.serializedObject.ApplyModifiedProperties();
        }

        protected void OnRegionListProviderChanged(SerializedProperty property)
        {
            if (property.boxedValue is not IRegionListProvider regionListProvider) { return; }

            GetChoices(regionListProvider, out List<AssetReference> sceneAssetReferences, out int index);
            dropdownField.choices = sceneAssetReferences;
            dropdownField.index = index;

            if (regionListProvider == null)
            {
                if (!root.Contains(dropdownField)) { return; }
                root.Remove(dropdownField);
            }
            else
            {
                if (root.Contains(dropdownField)) { return; }
                root.Add(dropdownField);
            }
        }

        protected void GetChoices(IRegionListProvider regionListProvider, out List<AssetReference> regionAssetReferences, out int index)
        {
            index = -1;
            regionAssetReferences = GetRegionAssetReferences(regionListProvider);

            if (regionListProvider != null && regionAssetReferenceProperty.boxedValue is AssetReference currentSceneAssetReference)
            {
                index = IndexOf(regionAssetReferences, currentSceneAssetReference);
            }
        }

        private List<AssetReference> GetRegionAssetReferences(IRegionListProvider regionListProvider)
        {
            if (regionListProvider == null)
            {
                return new List<AssetReference>();
            }

            AssetReference[] result = regionListProvider.Editor_RegionAssetReferences;
            return result.ToList();
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
