using System.Collections.Generic;
using KekwDetlef.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KekwDetlef.LOST
{
    public abstract class VerifiableComponentMultiple<TEditor, TRuntime> : VerifiableComponent where TEditor : IVerifiable<TRuntime>, ISceneListSettable
    {

#if UNITY_EDITOR
        [SerializeField] private SDictionary<BaseSceneList, SHashSet<TEditor>> verifiables = new SDictionary<BaseSceneList, SHashSet<TEditor>>();

        protected void OnValidate()
        {
            var newValue = verifiables.Editor_NewValue;
            if (newValue != null)
            {
                newValue.Value.Editor_NewValue.SetSceneList(newValue.Key);

                foreach (TEditor newVerifiable in newValue.Value)
                {
                    newVerifiable.SetSceneList(newValue.Key);
                }
            }

            foreach (var pair in verifiables)
            {
                foreach (TEditor newVerifiable in pair.Value)
                {
                    newVerifiable.SetSceneList(pair.Key);
                }
            }

            OnAfterValidate();
        }

        protected virtual void OnAfterValidate() { }

        // TODO: poll the error messages
        private bool GetRuntimeDatas(out TRuntime[] runtimeDatas, out string errorMessage)
        {
            List<TRuntime> datas = new List<TRuntime>();
            foreach (var pair in verifiables)
            {
                foreach (TEditor verifiable in pair.Value)
                {
                    if (verifiable.Verify(out TRuntime runtimeData, out errorMessage))
                    {
                        datas.Add(runtimeData);
                    }
                    else
                    {
                        runtimeDatas = null;
                        return false;
                    }
                }
            }

            runtimeDatas = datas.ToArray();
            errorMessage = null;
            return true;
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


    public abstract class VerifiableComponentMultipleSceneAssetReferences : VerifiableComponentMultiple<VerifiableSceneReferenceHiddenSceneList, AssetReference> { }
    public abstract class VerifiableComponentMultipleRegionLoadInfos : VerifiableComponentMultiple<VerifiableRegionLoadInfo<VerifiableSceneReferenceHiddenSceneList>, RegionLoadInfo> { }
}
