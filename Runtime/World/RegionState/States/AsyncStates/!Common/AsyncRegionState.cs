using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    internal abstract class AsyncRegionState : IRegionState
    {
        private CancellationTokenSource currentCts = null;
        private Task<IRegionState> currentTask = null;
        private Task executionTask = null;
        private Procedure currentProcedure = Procedure.Default;

        Task<IRegionState> IRegionState.Execute()
        {
            executionTask = OnExecute();
            return GetCurrentTask(Procedure.Default, OnExecutionFinished);
        }

        Task<IRegionState> IRegionState.Load(AssetReference sceneAssetReference, int priority) => GetCurrentTask(GetLoadProcedure(sceneAssetReference, priority), OnLoad);
        Task<IRegionState> IRegionState.Unload() => GetCurrentTask(GetUnloadProcedure(), OnUnload);

        protected abstract Task OnExecute();
        protected abstract IRegionState OnExecutionFinished();
        protected abstract IRegionState OnLoad();
        protected abstract IRegionState OnUnload();
        protected abstract Procedure GetLoadProcedure(AssetReference sceneAssetReference, int priority);
        protected abstract Procedure GetUnloadProcedure();

        private Task<IRegionState> GetCurrentTask(Procedure procedure, Func<IRegionState> getNewState)
        {
            if (currentTask != null && procedure == currentProcedure)
            {
                return currentTask;
            }

            currentTask = CreateNewTask(procedure, getNewState);
            return currentTask;
        }

        // PROBLEM: i am not quite sure this works how i want it to
        private async Task<IRegionState> CreateNewTask(Procedure procedure, Func<IRegionState> getNewState)
        {
            currentProcedure = procedure;

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
    }
}
