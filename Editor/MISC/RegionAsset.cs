using UnityEngine;

namespace KekwDetlef.LOST.Editor
{
    [CreateAssetMenu(fileName = "RegionAsset", menuName = "LOST/RegionAsset")]
    public class RegionAsset : ScriptableObject
    {
        [SerializeField] private RegionLoadedCallback callback = null;
        [SerializeField] private SceneAssetReference sceneAssetReference = null;
    }
}
