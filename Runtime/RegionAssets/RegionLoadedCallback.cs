using UnityEngine;

namespace KekwDetlef.LOST
{
    public abstract class RegionLoadedCallback : ScriptableObject
    {
        internal void Run(GameObject[] rootGameObjects) => OnRun(rootGameObjects);
        protected abstract void OnRun(GameObject[] rootGameObjects);
    }
}
