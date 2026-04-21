using UnityEngine;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// 
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
            // TODO: document that this name is taken
            Scene viewport = SceneManager.CreateScene("Viewport");
            if (viewport.IsValid())
            {
                this.viewport = viewport;
                return;
            }

            // TODO: what if creating the scene faild
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            instance ??= new Viewport();
        }
#endregion // Singleton

        private readonly Scene viewport;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public T Instantiate<T>(T original) where T : Widget => (T)Object.Instantiate(original, viewport);

        /// <summary>
        /// 
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
