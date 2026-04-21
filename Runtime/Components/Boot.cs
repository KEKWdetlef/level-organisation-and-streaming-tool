using KekwDetlef.LOST;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private InitializeWorld initializeWorld;

    void Start()
    {
        initializeWorld.Run();
        Viewport.Initialize();
    }
}
