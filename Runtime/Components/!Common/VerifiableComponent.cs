using UnityEditor;
using UnityEngine;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// Base class used by LOST to implement components that talk to the "WorldState" api. 
    /// Objects of this class are discovered by the LOST packge when a verification is requested and before any build.
    /// </summary>
    public abstract class VerifiableComponent : MonoBehaviour
    {

#if UNITY_EDITOR
        private bool isWaitingForUnpause = false;
        
        /// <summary>
        /// Checks if the data of this object is valid. If not an error message describing the issue is printed to the console.
        /// <para />
        /// Called by the LOST package when a verification is requested and before any build. 
        /// </summary>
        /// <returns> Returns true if successful, otherwise false. </returns>
        public bool Editor_Verify()
        {
            bool isValid = Editor_OnVerify(out string errorMessage);
            if (!isValid)
            {
                Editor_LogError(errorMessage);
            }

            return isValid;
        }

        /// <summary>
        /// WARNING: AN IMPLEMENTATION OF THIS FUNCTION MUST BE GUAREDED BY (#if UNITY_EDITOR) PREPROCESSOR DIRECTIVES.
        /// <para />
        /// 
        /// Implement this function to verify and cook this objects data for a build. This function will be executed automatically.
        /// </summary>
        /// <param name="errorMessage"> Error message to be printed to the console in the case that the verification faild. 
        /// Expected to be valid if return value is false, otherwise invalid. 
        /// </param>
        /// <returns> Should return false if the verification failed, otherwise true. </returns>
        protected virtual bool Editor_OnVerify(out string errorMessage)
        {
            errorMessage = null;
            return true;
        }


        // TOOD: test if this works the way i want it to.
        private void Editor_Run()
        {
            if (Editor_OnRun(out string errorMessage)) { return; }

            EditorApplication.isPaused = true;
            Editor_LogError(errorMessage);
            Selection.activeGameObject = gameObject;
            EditorGUIUtility.PingObject(gameObject);

            if (isWaitingForUnpause) { return; }

            isWaitingForUnpause = true;
            EditorApplication.pauseStateChanged += OnEditorPauseStateChanged;
        }

        private void OnEditorPauseStateChanged(PauseState state)
        {
            // cursed unity null check
            if (this == null)
            {
                EditorApplication.pauseStateChanged -= OnEditorPauseStateChanged;
                return;
            }

            if (!EditorApplication.isPlaying || state != PauseState.Unpaused) { return; }

            EditorApplication.pauseStateChanged -= OnEditorPauseStateChanged;
            isWaitingForUnpause = false;

            Editor_Run();
        }

        /// <summary>
        /// WARNING: AN IMPLEMENTATION OF THIS FUNCTION MUST BE GUAREDED BY (#if UNITY_EDITOR) PREPROCESSOR DIRECTIVES.
        /// <para />
        /// 
        /// This method is called instead of "OnRun" when run in the Unity editor. 
        /// If this method returns false as the result, the play-mode will be paused and the out "errorMessage" will be logged as an error to the console.
        /// 
        /// <para />
        /// While the play-mode is paused, the user may fix the issue and return to play-mode. The operation will be rerun. 
        /// Unless the issue has been fixed or the user has exited the play mode, the play-mode will, again, be paused, unill the issue is fixed.
        /// 
        /// </summary>
        /// <param name="errorMessage"> Error message to be printed to the console in the case that the execution faild. 
        /// Expected to be valid if return value is false, otherwise invalid. 
        /// </param>
        /// <returns> Returns false if the execution failed, otherwise true. </returns>
        protected abstract bool Editor_OnRun(out string errorMessage);

        private void Editor_LogError(string errorMessage)
        {
            // TODO: test if this works fine
            // TODO: im sure i can find some more usefull information to add here.
            string quallifiedErrorMessage = $"{gameObject.scene.name}/{gameObject.name}/{GetType().Name}\n\n{errorMessage}";
            Debug.LogError(quallifiedErrorMessage, this);
        }
#endif // UNITY_EDITOR

        /// <summary>
        /// Executes whatever functionality is provided by the implementation.
        /// </summary>
        public void Run()
        {
            
#if UNITY_EDITOR
            Editor_Run();
#else
            OnRun();
#endif // UNITY_EDITOR

        }

        /// <summary>
        /// This method is called instead of "Editor_OnRun" when not run in the Unity editor. (So eg. in a build)
        /// </summary>
        protected abstract void OnRun();
    }
}
