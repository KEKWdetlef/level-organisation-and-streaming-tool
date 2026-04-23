using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

// TODO: wrap scene manager functionality, so that i can confidantly say you should not use scene manager for anything
// TODO: check if the a providet scene for anything with the regions is the current level scene.
namespace KekwDetlef.LOST
{
    /// <summary>
    /// Singleton representing the state of the world, meaning current level and regions. Main API for any world manipulation such as loading a level, region, etc..
    /// </summary>
    public class World
    {
    
#region Singleton
        private static World instance = null;
 
        private World(AssetReference initialLevel)
        {
            Scene emptyVoid = SceneManager.CreateScene("EmptyVoid");
            if (!emptyVoid.IsValid())
            {
                Helper.FailedToCreateScene();
                return;
            }

            LoadFirstLevel(initialLevel);

            // todo: fix the timer cuz i am certain this does not work yet the way i intend it to
            gcTimer = new Timer
            {
                // TODO: make this a mutable value in the project settings
                Interval = 5000,
                AutoReset = true
            };

            gcTimer.Elapsed += CollectGarbage;
            gcTimer.Start();

            // TODO: chat gpt says that "Application.quitting" might never be called on some devices such as mobile phones. potentioally unsafe. 
            Application.quitting += Dispose;
        }

        /// <summary>
        /// Initializes the world state and loads the initial level. (Does not unload the scene this is initialzed from, 
        /// for example the a bootstrap scene.)
        /// </summary>
        /// <param name="initialLevel"> The scene that is initialy loaded as a level. Crashes if not valid or not a scene. </param>
        /// <returns> Returns false if the world state is already initialized, otherwise true. </returns>
        public static bool Initialize(AssetReference initialLevel) 
        {
            if (instance != null) { return false; }

            instance = new World(initialLevel);
            return true;
        }

        /// <summary>
        /// Gets reference to the instance of the "WorldState" singleton. 
        /// </summary>
        /// <param name="instance"> Outputs the singleton instance of the "WorldState". Null if return value is false. </param>
        /// <returns> Whether the instance is valid. Invalid if the "WorldState" has not been initialized. </returns>
        public static bool GetInstance(out World instance)
        {
            instance = World.instance;
            return World.instance != null;
        }
#endregion // Singleton

        private readonly Timer gcTimer;

        private Region currentLevelRegionHandle;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<int, Region> regionsByHash = new Dictionary<int, Region>();

        private bool isTearingDown = false;
        private bool isLoadingNewLevel = false;

        private async void LoadFirstLevel(AssetReference sceneAssetReference)
        {
            if (currentLevelRegionHandle != null) { return; }

            isLoadingNewLevel = true;
            
            Region newRegionHandle = new Region(sceneAssetReference);
            await newRegionHandle.Load(priority: 100, shouldReload: false);
            currentLevelRegionHandle = newRegionHandle;

            isLoadingNewLevel = false;
        }

        public async void LoadLevel(AssetReference sceneAssetReference)
        {
            if (isLoadingNewLevel || isTearingDown) { return; }

            isLoadingNewLevel = true;
            await LoadLevelInternal(sceneAssetReference);
            isLoadingNewLevel = false;
        }

        private async Task LoadLevelInternal(AssetReference sceneAssetReference)
        {
            await TearDown();

            if (IsCurrentlyLoadedLevel(sceneAssetReference))
            {
                await currentLevelRegionHandle.Load(priority: 100, shouldReload: true);
            }
            else
            {
                Region newLevelRegionHandle = new Region(sceneAssetReference);

                Task unloadTask = currentLevelRegionHandle?.Unload();
                Task loadTask = newLevelRegionHandle.Load(priority: 100, shouldReload: true);

                await Task.WhenAll(new [] {unloadTask, loadTask});
                currentLevelRegionHandle = newLevelRegionHandle;
            }
        }

        private async Task TearDown()
        {
            isTearingDown = true;

            Task[] tasks = UnloadAllRegionsInternal();
            await currentLevelRegionHandle?.Unload();
            await Task.WhenAll(tasks);

            isTearingDown = false;
        }

        public void SetRegionsLoaded(RegionLoadInfo[] loadInfos)
        {
            if (isTearingDown) { return; }

            int length = loadInfos.Length;
            AssetReference[] exceptions = new AssetReference[length];

            for (int i = 0; i < length; i++)
            {
                RegionLoadInfo loadInfo = loadInfos[i];
                exceptions[i] = loadInfo.SceneAssetReference;
            }

            UnloadAllRegions(exceptions);
            LoadRegions(loadInfos);
        }

        public void LoadRegions(RegionLoadInfo[] loadInfos)
        {
            if (isTearingDown) { return; }

            AssetReferenceGuidComparer comparer = new AssetReferenceGuidComparer();

            foreach (RegionLoadInfo loadInfo in loadInfos)
            {
                int hashCode = comparer.GetHashCode(loadInfo.SceneAssetReference);
                if (regionsByHash.TryGetValue(hashCode, out Region regionHandle))
                {
                    _ = regionHandle.Load(loadInfo.Priority, loadInfo.ShouldReload);
                }
                else
                {
                    Region newRegionHandle = new Region(loadInfo.SceneAssetReference);
                    _ = newRegionHandle.Load(loadInfo.Priority, loadInfo.ShouldReload);
                    regionsByHash.Add(newRegionHandle.GetHashCode(), newRegionHandle);
                }
            }
        }

        public void UnloadRegions(AssetReference[] sceneAssetReferences)
        {
            if (isTearingDown) { return; }

            AssetReferenceGuidComparer comparer = new AssetReferenceGuidComparer();

            foreach (AssetReference sceneAssetReference in sceneAssetReferences)
            {
                int hashCode = comparer.GetHashCode(sceneAssetReference);
                if (regionsByHash.TryGetValue(hashCode, out Region regionHandle))
                {
                    _ = regionHandle.Unload();
                }
            }
        }

        public void UnloadAllRegions()
        {
            if (isTearingDown) { return; }

            UnloadAllRegionsInternal();
        }

        private Task[] UnloadAllRegionsInternal()
        {
            Task[] result = new Task[regionsByHash.Count];
            
            int index = 0;
            foreach (var regionHandle in regionsByHash)
            {
                result[index] = regionHandle.Value.Unload();
                index++;
            }
            return result;
        }

        private Task[] UnloadAllRegions(AssetReference[] exceptions)
        {
            HashSet<int> regionHandleHashCodes = regionsByHash.Keys.ToHashSet();
            foreach(AssetReference exception in exceptions)
            {
                regionHandleHashCodes.Remove(new AssetReferenceGuidComparer().GetHashCode(exception));
            }

            Task[] result = new Task[regionHandleHashCodes.Count];
            
            int index = 0;
            foreach (int hashCode in regionHandleHashCodes)
            {
                Region regionHandle = regionsByHash[hashCode];
                result[index] = regionHandle.Unload();
                index++;
            }
            
            return result;
        }

        // i think that this might be unsafe cuz runs on different thread maybe (honest no clue tho)
        private void CollectGarbage(object sender, ElapsedEventArgs e)
        {
            List<int> freeHandlesHash = new List<int>();
            foreach (var pair in regionsByHash)
            {
                Region regionHandle = pair.Value;

                if (regionHandle.IsUnloaded)
                {
                    freeHandlesHash.Add(pair.Key);
                }
            }

            foreach (int hash in freeHandlesHash)
            {
                regionsByHash.Remove(hash);
            }
        }

        private void Dispose()
        {
            gcTimer.Dispose();
        }

        private bool IsCurrentlyLoadedLevel(AssetReference sceneAssetReference) => new AssetReferenceGuidComparer().GetHashCode(sceneAssetReference) == currentLevelRegionHandle.GetHashCode();
    }
}
