namespace KekwDetlef.LOST
{
    public class UnloadAllRegions : VerifiableComponent
    {
#if UNITY_EDITOR
        protected override bool Editor_OnRun(out string errorMessage)
        {
            if (WorldState.GetInstance(out WorldState instance))
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
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.UnloadAllRegions();
            }
        }
    }
}
