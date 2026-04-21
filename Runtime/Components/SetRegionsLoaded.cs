using UnityEngine;

namespace KekwDetlef.LOST
{
    public class SetRegionsLoaded : VerifiableComponentMultipleRegionLoadInfos
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(RegionLoadInfo[] regionLoadInfos, out string errorMessage)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.SetRegionsLoaded(regionLoadInfos);
                errorMessage = null;
                return true;
            }

            errorMessage = Helper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR
        protected override void OnRun(RegionLoadInfo[] regionLoadInfos)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.SetRegionsLoaded(regionLoadInfos);
            }
        }
    }
}
