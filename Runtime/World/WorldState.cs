using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KekwDetlef.LOST
{
    /// <summary>
    /// Singleton representing the state of the world, meaning current level and regions. Main API for any world manipulation such as loading a level, region, etc..
    /// </summary>
    public class WorldState
    {
    
#region Singleton
        private static WorldState instance = null;
 
        private WorldState() { }

        /// <summary>
        /// Initializes the world state and loads the initial level. (Does not unload the scene this is initialzed from, 
        /// for example the a bootstrap scene.)
        /// </summary>
        /// <param name="initialLevel"> The scene that is initialy loaded as a level. Crashes if not valid or not a scene. </param>
        /// <returns> Returns false if the world state is already initialized, otherwise true. </returns>
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

        /// <summary>
        /// Gets reference to the instance of the "WorldState" singleton. 
        /// </summary>
        /// <param name="instance"> Outputs the singleton instance of the "WorldState". Null if return value is false. </param>
        /// <returns> Whether the instance is valid. Invalid if the "WorldState" has not been initialized. </returns>
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
