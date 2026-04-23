using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal abstract class AsyncRegionLoadState : RegionLoadState
    {
        private CancellationTokenSource currentCts = null;
        private Task<RegionLoadState> currentTask = null;
        private Task executionTask = null;
        private Procedure currentProcedure = Procedure.Default;

        protected sealed override void OnDispose() => currentCts?.Dispose();

        internal sealed override Task<RegionLoadState> Execute()
        {
            executionTask = OnExecute();
            return GetCurrentTask(Procedure.Default, OnExecutionFinished);
        }

        internal sealed override Task<RegionLoadState> Load(AssetReference sceneAssetReference, int priority, bool shouldReload) => GetCurrentTask(GetLoadProcedure(sceneAssetReference, priority), OnLoad);
        internal sealed override Task<RegionLoadState> Unload() => GetCurrentTask(GetUnloadProcedure(), OnUnload);

        private Task<RegionLoadState> GetCurrentTask(Procedure procedure, Func<RegionLoadState> getNewState)
        {
            if (currentTask != null && procedure == currentProcedure)
            {
                return currentTask;
            }

            currentTask = CreateNewTask(procedure, getNewState);
            return currentTask;
        }

        // PROBLEM: i am not quite sure this works how i want it to
        private async Task<RegionLoadState> CreateNewTask(Procedure newProcedure, Func<RegionLoadState> getNewState)
        {
            currentProcedure = newProcedure;

            CancellationTokenSource previousCts = currentCts;
            previousCts?.Cancel();
            previousCts?.Dispose();

            CancellationTokenSource nextCts = new CancellationTokenSource();
            currentCts = nextCts;

            CancellationToken token = currentCts.Token;
            Task cancelTask = Task.Delay(Timeout.Infinite, token);

            await Task.WhenAny(executionTask, cancelTask);
            token.ThrowIfCancellationRequested();

            return getNewState?.Invoke();
        }

        protected abstract Task OnExecute();
        protected abstract RegionLoadState OnExecutionFinished();
        protected abstract RegionLoadState OnLoad();
        protected abstract RegionLoadState OnUnload();
        protected abstract Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority);
        protected abstract Procedure GetUnloadProcedure();
    }

}
