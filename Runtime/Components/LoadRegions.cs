using UnityEngine;

namespace KekwDetlef.LOST
{
    public class LoadRegions : VerifiableComponentMultipleRegionLoadInfos
    {

#if UNITY_EDITOR
        protected override bool Editor_OnRun(RegionLoadInfo[] regionLoadInfos, out string errorMessage)
        {
            if (World.GetInstance(out World instance))
            {
                instance.LoadRegions(regionLoadInfos);
                errorMessage = null;
                return true;
            }

            errorMessage = Helper.InvalidWorldStateErrorMessage;
            return false;
        }
#endif // UNITY_EDITOR

        protected override void OnRun(RegionLoadInfo[] regionLoadInfos)
        {
            if (World.GetInstance(out World instance))
            {
                instance.LoadRegions(regionLoadInfos);
            }
        }
    }
}
