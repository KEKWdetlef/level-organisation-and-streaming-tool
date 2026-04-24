using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    internal static class Helper
    {
        internal static string InvalidWorldStateErrorMessage => "TODO: write InvalidWorldStateErrorMessage";

        // TODO: Improve this pls
        internal static void AssertHandleValid(AsyncOperationHandle handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new System.Exception();
            }
        }

        internal static void FailedToCreateScene()
        {
            throw new System.Exception("TODO: write failed to create scene exception");
        }
    }
}
