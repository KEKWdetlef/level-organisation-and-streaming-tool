using UnityEngine;
using UnityEngine.UI;

namespace KekwDetlef.LOST
{
    [RequireComponent(typeof(Canvas))]
    public class Widget : MonoBehaviour
    {
        protected virtual void Reset()
        {
            if (TryGetComponent(out Canvas canvas))
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            if (!TryGetComponent(out CanvasScaler scaler))
            {
                gameObject.AddComponent<CanvasScaler>();
            }

            if (!TryGetComponent(out GraphicRaycaster raycaster))
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }
        }
    }
}
