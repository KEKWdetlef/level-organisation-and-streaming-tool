using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KekwDetlef.LOST.Editor
{
    internal class LevelListWindow : EditorWindow
    {
        [MenuItem("Window/LOST/LevelList")]
        private static void CreateWindow()
        {
            LevelListWindow window = GetWindow<LevelListWindow>();
            window.titleContent = new GUIContent("LevelListWindow");
        }

        private void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);
        }
    }
}
