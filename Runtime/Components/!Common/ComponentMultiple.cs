using System.Collections.Generic;
using KekwDetlef.SerializedCollections;
using UnityEngine;

namespace KekwDetlef.LOST
{
    public abstract class ComponentMultiple<TEditor, TRuntime, TProvider> : VerifiableComponent
                                                                          , IRegionListProviderSettable<TProvider>
    where TEditor : IVerifiable<TRuntime>, IRegionListProviderSettable<TProvider>
    where TProvider : ScriptableObject, IRegionListProvider
    {

        // TODO: fix this cuz needs to be in editor guards
        public void SetRegionListProvider(TProvider regionListProvider)
        {

#if UNITY_EDITOR
            this.regionListProvider = regionListProvider;
#endif // UNITY_EDITOR

        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private TProvider regionListProvider = null;
        [SerializeField] private SHashSet<TEditor> verifiables = new SHashSet<TEditor>();

        protected void OnValidate()
        {
            var newValue = verifiables.Editor_NewValue;
            newValue?.SetRegionListProvider(regionListProvider);

            foreach (TEditor verifiable in verifiables)
            {
                verifiable.SetRegionListProvider(regionListProvider);
            }

            OnAfterValidate();
        }

        protected virtual void OnAfterValidate() { }

        private bool GetRuntimeDatas(out TRuntime[] runtimeDatas, out string errorMessage)
        {
            bool result = true;
            errorMessage = string.Empty;

            List<TRuntime> datas = new List<TRuntime>();
            foreach (TEditor verifiable in verifiables)
            {
                if (verifiable.Verify(out TRuntime runtimeData, out string verifyErrorMessage))
                {
                    datas.Add(runtimeData);
                }
                else
                {
                    errorMessage += $"\n\n {verifyErrorMessage}";
                    result = false;
                }
            }

            if (result)
            {
                runtimeDatas = datas.ToArray();
                errorMessage = null;
                return true;
            }

            runtimeDatas = null;
            return false;
        }

        protected sealed override bool Editor_OnVerify(out string errorMessage)
        {
            if (GetRuntimeDatas(out TRuntime[] runtimeDatas, out errorMessage))
            {
                this.runtimeDatas = runtimeDatas;
                return true;
            }

            return false;
        }

        protected sealed override bool Editor_OnRun(out string errorMessage)
        {
            if (GetRuntimeDatas(out TRuntime[] runtimeDatas, out errorMessage))
            {
                return Editor_OnRun(runtimeDatas, out errorMessage);
            }

            return false;
        }

        protected abstract bool Editor_OnRun(TRuntime[] runtimeDatas, out string errorMessage);
#endif // UNITY_EDITOR

        [SerializeField, HideInInspector] private TRuntime[] runtimeDatas = null;

        protected sealed override void OnRun() => OnRun(runtimeDatas);
        protected abstract void OnRun(TRuntime[] runtimeDatas);
    }
}
