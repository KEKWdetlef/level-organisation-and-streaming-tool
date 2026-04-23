using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public abstract class Boot : MonoBehaviour
    {
        [SerializeField] protected InitializeWorld initializeWorld;

    #if UNITY_EDITOR
        internal abstract void Editor_Run(AssetReference sceneAssetReference);
    #else
        protected void Start() => Run();
        protected abstract void Run();
    #endif // UNITY_EDITOR

    }
}
