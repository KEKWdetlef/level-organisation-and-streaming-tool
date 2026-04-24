using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// Base class for defining boot behaviour. LOST requires there to be exactly one boot object in the boot scene. 
    /// See <see cref="DefaultBoot"/> for the default boot implementation. 
    /// </summary>
    public abstract class Boot : MonoBehaviour
    {
        [SerializeField] protected InitializeWorld initializeWorld;

    #if UNITY_EDITOR
        /// <summary>
        /// Runs the boot script in editor with the option of overriding the default first level to load. Note that this is not possible in a build, 
        /// as i a build the the boot script runs automatically on start. 
        /// </summary>
        /// <param name="sceneAssetReference"> The scene asset reference loaded as the first level. Invalid if the default scene asset reference should be used. </param>
        internal void Editor_Run(AssetReference sceneAssetReference) => Editor_OnRun(sceneAssetReference);
         
        /// <summary>
        /// WARNING: AN IMPLEMENTATION OF THIS FUNCTION MUST BE GUAREDED BY (#if UNITY_EDITOR) PREPROCESSOR DIRECTIVES.
        /// <para />
        /// 
        /// Function to be implemented to run the boot script in editor. This function will be executed automatically.
        /// </summary>
        /// <param name="sceneAssetReference"> The scene asset reference loaded as the first level. Invalid if the default scene asset reference should be used. </param>
        protected abstract void Editor_OnRun(AssetReference sceneAssetReference);
    #else
        protected void Start() => Run();

        /// <summary>
        /// WARNING: AN IMPLEMENTATION OF THIS FUNCTION MUST BE GUAREDED BY (#if !UNITY_EDITOR) PREPROCESSOR DIRECTIVES.
        /// <para />
        /// 
        /// Run serves as an alias to Start. Note that, in a build, this function is automatically executed on Start. 
        /// In the editor the function "Editor_Run" should instead be used for execution.
        /// </summary>
        protected abstract void Run();
    #endif // UNITY_EDITOR

    }
}
