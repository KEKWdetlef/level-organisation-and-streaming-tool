using UnityEngine.ResourceManagement.AsyncOperations;

namespace KekwDetlef.LOST
{
    internal static class LOSTHelper
    {
        // TODO: Improve this pls
        internal static void AssertHandleValid(AsyncOperationHandle handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new System.Exception();
            }
        }
    }
}
