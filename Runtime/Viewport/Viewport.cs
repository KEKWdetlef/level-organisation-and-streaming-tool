using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    public class Viewport
    {

#region Singleton
        private static Viewport instance = null;

        public static Viewport Instance
        {
            get
            {
                instance ??= InitializeInternal();
                return instance;
            }
        }

        private Viewport() { }

        public static void Initialize()
        {
            instance ??= InitializeInternal();
        }

        private static Viewport InitializeInternal()
        {
            Viewport viewport = new Viewport();
            viewport.Create();
            return viewport;
        }
#endregion // Singleton

        private Scene viewport;

        private void Create()
        {
            Scene viewport = SceneManager.CreateScene("Viewport");
            if (viewport.IsValid())
            {
                this.viewport = viewport;
                return;
            }

            // TODO: what if creating the scene faild
        }

        public T Add<T>(T original) where T : Object => (T)Object.Instantiate(original, viewport);

        public void RemoveAll()
        {
            GameObject[] widgets = viewport.GetRootGameObjects();
            foreach (Object widget in widgets)
            {
                Object.Destroy(widget);
            }
        }
    }
}
