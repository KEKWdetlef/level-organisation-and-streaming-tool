using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    public class WorldState
    {
    
#region Singleton
        private static WorldState instance = null;

        private WorldState() { }

        internal static void Initialize(AssetReference initialLevel) 
        {
            if (instance != null) 
            {
                // should probaby throw error here or smthing 
                return; 
            }

            instance = new WorldState();

            instance.LoadLevel(initialLevel);
        }

        public static bool GetInstance(out WorldState instance)
        {
            instance = WorldState.instance;
            return WorldState.instance != null;
        }
#endregion // Singleton

        private RegionHandle levelHandle;
        private readonly HashSet<RegionHandle> regionHandles = new HashSet<RegionHandle>();

        public void LoadLevel(AssetReference sceneAssetReference)
        {
            Scene test = SceneManager.CreateScene("test");
            UnloadAllRegions();

            RegionHandle newLevelHandle = new RegionHandle(sceneAssetReference);
            if (levelHandle.Equals(newLevelHandle))
            {
                levelHandle.Load(LoadSceneMode.Single, priority: 100);
            }
            else
            {
                levelHandle.Unload();
                newLevelHandle.Load(LoadSceneMode.Single, priority: 100);
                levelHandle = newLevelHandle;
            }

            SceneManager.UnloadSceneAsync(test);
        }

        private void UnloadAllRegions()
        {
            foreach (RegionHandle regionHandle in regionHandles)
            {
                regionHandle.Unload();
            }
        }

        private void UnloadAllRegions(AssetReference[] exceptions)
        {
            foreach (RegionHandle regionHandle in regionHandles)
            {
                if (AnyEquals(regionHandle, exceptions)) { return; }

                regionHandle.Unload();
            }
        }

        private bool AnyEquals(RegionHandle regionHandle, AssetReference[] exceptions)
        {
            foreach (AssetReference exception in exceptions)
            {
                if (regionHandle.Equals(exception))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
