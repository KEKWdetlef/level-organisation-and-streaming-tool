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
                instance ??= new Viewport();
                return instance;
            }
        }

        private Viewport()
        {
            // TODO: document that this name is taken
            Scene viewport = SceneManager.CreateScene("Viewport");
            if (viewport.IsValid())
            {
                this.viewport = viewport;
                return;
            }

            // TODO: what if creating the scene faild
        }

        public static void Initialize()
        {
            instance ??= new Viewport();
        }
#endregion // Singleton

        private readonly Scene viewport;

        public T Add<T>(T original) where T : Object => (T)Object.Instantiate(original, viewport);

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
