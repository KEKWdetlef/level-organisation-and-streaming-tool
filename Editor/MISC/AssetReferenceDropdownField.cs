using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    public class AssetReferenceDropdownField : PopupField<AssetReference>
    {
        public AssetReferenceDropdownField(string fieldName, List<AssetReference> data)
        {
            label = fieldName;
            choices = data;
            index = -1;

            formatSelectedValueCallback = FormatItem;
            formatListItemCallback = FormatItem;
        }

        public AssetReferenceDropdownField(string fieldName, List<AssetReference> data, int index)
        {
            label = fieldName;
            choices = data;
            this.index = index;

            formatSelectedValueCallback = FormatItem;
            formatListItemCallback = FormatItem;
        }

        private string FormatItem(AssetReference reference)
        {
            if (reference == null || reference.editorAsset == null || reference.editorAsset.name == null)
            {
                return "Invalid";
            }

            return reference.editorAsset.name;
        }
    }
}
