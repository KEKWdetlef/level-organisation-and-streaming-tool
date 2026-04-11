using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    public class WorldState
    {
    
#region Singleton
        private static WorldState instance = null;

        private WorldState() { }

        public static bool Initialize(AssetReference initialLevel) 
        {
            if (instance != null) 
            {
                // should probaby throw error here or smthing 
                return false; 
            }

            instance = new WorldState();
            instance.LoadLevel(initialLevel);
            return true;
        }

        public static bool GetInstance(out WorldState instance)
        {
            instance = WorldState.instance;
            return WorldState.instance != null;
        }
#endregion // Singleton

        private RegionHandle levelHandle;
        private readonly HashSet<RegionHandle> regionHandles = new HashSet<RegionHandle>();

        private bool isTearingDown = false;
        private bool isLoadingNewLevel = false;

        public async void LoadLevel(AssetReference sceneAssetReference)
        {
            if (isLoadingNewLevel) { return; }

            isLoadingNewLevel = true;
            await LoadLevelInternal(sceneAssetReference);
            isLoadingNewLevel = false;
        }

        private async Task LoadLevelInternal(AssetReference sceneAssetReference)
        {
            // TODO: just have this be a random combination of letters and numbers also maybe document that this name is reserved for the package (can not be used by user)
            Scene tempScene = SceneManager.CreateScene("TemporaryVoidScene");

            await TearDown();

            RegionHandle newLevelHandle = new RegionHandle(sceneAssetReference);
            if (levelHandle.Equals(newLevelHandle))
            {
                await levelHandle.Load(priority: 100);
            }
            else
            {
                Task unloadTask = levelHandle.Unload();
                Task loadTask = newLevelHandle.Load(priority: 100);

                await Task.WhenAll(new [] {unloadTask, loadTask});
                levelHandle = newLevelHandle;
            }

            SceneManager.UnloadSceneAsync(tempScene);
        }

        private async Task TearDown()
        {
            isTearingDown = true;

            List<Task> tasks = UnloadAllRegions();
            tasks.Add(levelHandle.Unload());
            await Task.WhenAll(tasks);

            isTearingDown = false;
        }

        private List<Task> UnloadAllRegions()
        {
            List<Task> result = new List<Task>();
            foreach (RegionHandle regionHandle in regionHandles)
            {
                Task task = regionHandle.Unload();
                result.Add(task);
            }
            return result;
        }

        private List<Task> UnloadAllRegions(AssetReference[] exceptions)
        {
            List<Task> result = new List<Task>();
            foreach (RegionHandle regionHandle in regionHandles)
            {
                if (AnyEquals(regionHandle, exceptions)) { continue; }

                Task task = regionHandle.Unload();
                result.Add(task);
            }
            return result;
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
