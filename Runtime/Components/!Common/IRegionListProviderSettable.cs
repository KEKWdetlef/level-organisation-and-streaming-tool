using UnityEngine;

namespace KekwDetlef.LOST
{
    public interface IRegionListProviderSettable<TProvider> where TProvider : ScriptableObject, IRegionListProvider
    {
        public void SetRegionListProvider(TProvider regionListProvider);
    }
}
