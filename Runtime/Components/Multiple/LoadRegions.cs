using UnityEngine;

namespace KekwDetlef.LOST
{
    public class LoadRegions : VerifiableComponentMultipleRegionLoadInfos
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(RegionLoadInfo[] regionLoadInfos, out string errorMessage)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.LoadRegions(regionLoadInfos);
                errorMessage = null;
                return true;
            }

            errorMessage = LOSTHelper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(RegionLoadInfo[] regionLoadInfos)
        {
            if (WorldState.GetInstance(out WorldState instance))
            {
                instance.LoadRegions(regionLoadInfos);
            }
        }
    }
}
