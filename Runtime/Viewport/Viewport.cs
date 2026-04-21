using UnityEngine;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// Provides functionality to add User Interface in the form of <see cref="Widget"/>s to the game. 
    /// Acts as a lazy initialized singleton.
    /// </summary>
    public class Viewport
    {

#region Singleton
        private static Viewport instance = null;

        public static Viewport Instance
        {
            get
            {
                instance ??= new Viewport();
                return instance;
            }
        }

        private Viewport()
        {
            Scene viewport = SceneManager.CreateScene("Viewport");
            if (!viewport.IsValid())
            {
                Helper.FailedToCreateScene();
                return;
            }

            this.viewport = viewport;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        /// <summary>
        /// Initializes the viewport. Can be omited if you want the viewport to be Initialized lazy.
        /// </summary>
        public static void Initialize()
        {
            instance ??= new Viewport();
        }
#endregion // Singleton

        private readonly Scene viewport;

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            if (viewport.handle.Equals(unloadedScene.handle))
            {
                throw new System.InvalidOperationException("The viewport may not be unloaded, unless the game is shuting down.");
            }
        }

        private void OnActiveSceneChanged(Scene oldActive, Scene newActive)
        {
            if (viewport.handle.Equals(newActive.handle))
            {
                // TODO: i dont like throwing exception here
                throw new System.InvalidOperationException("The viewport may not be made the active scene.");
            }
        }

        /// <summary>
        /// Creates a widget on the viewport based on an original and returns the new instance.
        /// </summary>
        /// <typeparam name="T"> The widget type of the object that is created and returned. </typeparam>
        /// <param name="original"> The template from which the widget is created. E.g. a prefab. </param>
        /// <returns> Returns the new instance. </returns>
        public T CreateWidget<T>(T original) where T : Widget => (T)Object.Instantiate(original, viewport);

        /// <summary>
        /// Removes all widgets currently active in the viewport. Use carefully as all references will get brocken.
        /// </summary>
        public void RemoveAll()
        {
            GameObject[] objects = viewport.GetRootGameObjects();
            foreach (Object obj in objects)
            {
                Object.Destroy(obj);
            }
        }
    }
}
