using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] WorldController worldController = null;
    [SerializeField] Camera currCamera = null;
    [SerializeField] GameObject cameraOrigin_go = null;
    [SerializeField] float minScale = 3;
    [SerializeField] float maxScale = 25;

    ICameraUpdateBehaviour[] cameraUpdateBehaviours;
    ICursorProvider cursor;

    ConstrainedCamera constrainedCamera;

    void Awake()
    {
        cameraUpdateBehaviours = GetComponents<ICameraUpdateBehaviour>();
        cursor = GetComponent<ICursorProvider>();

        worldController.RegisterWorldCreatedCallback(Initialize);
    }

    void Initialize(NodeGrid<Tile> world)
    {
        int width = world.GridSizeX;
        int height = world.GridSizeY;

        Vector2 margin = new Vector2(1, 1);
        Vector2 cameraMinExtent = margin;
        Vector2 cameraMaxExtent = new Vector2(width, height) - margin;

        constrainedCamera = new ConstrainedCamera(currCamera, cameraMinExtent, cameraMaxExtent, minScale, maxScale) {
            Position = new Vector3(cameraOrigin_go.transform.position.x,
                                   cameraOrigin_go.transform.position.y,
                                   currCamera.transform.position.z)
        };
    }

    void Update()
    {
        if (!cursor.IsPointerOutOfFrame) {
            foreach (ICameraUpdateBehaviour updateBehaviour in cameraUpdateBehaviours) {
                updateBehaviour.UpdateCamera(ref constrainedCamera, ref cursor);
            }
        }
    }
}
