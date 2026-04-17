using UnityEditor.Build;

namespace KekwDetlef.LOST.Editor
{
    internal class PreBuildProcessor : IPreprocessBuildWithContext
    {
        // TODO: add order to project settings
        int IOrderedCallback.callbackOrder => 0;

        void IPreprocessBuildWithContext.OnPreprocessBuild(BuildCallbackContext ctx)
        {
            Verifier verifier = new Verifier();
            if (!verifier.Run())
            {
                throw new BuildFailedException("TODO: write exeption for when the build failed");
            }
        }
    }
}
