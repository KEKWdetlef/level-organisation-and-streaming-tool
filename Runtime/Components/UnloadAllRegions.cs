
namespace KekwDetlef.LOST
{
    public class UnloadAllRegions : VerifiableComponent
    {
#if UNITY_EDITOR
        protected override bool Editor_OnRun(out string errorMessage)
        {
            if (World.GetInstance(out World instance))
            {
                instance.UnloadAllRegions();
                errorMessage = null;
                return true;
            }

            errorMessage = Helper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun()
        {
            if (World.GetInstance(out World instance))
            {
                instance.UnloadAllRegions();
            }
        }
    }
}
