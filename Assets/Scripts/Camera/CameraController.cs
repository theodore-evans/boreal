using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera currCamera = null;
    [SerializeField] GameObject cameraOrigin_go = null;
    [SerializeField] GameObject worldController_go = null;
    [SerializeField] float minScale = 3;
    [SerializeField] float maxScale = 25;

    ICameraUpdateBehaviour[] cameraUpdateBehaviours;
    ICursorProvider cursor;

    ConstrainedCamera constrainedCamera;

    void Start()
    {
        cameraUpdateBehaviours = GetComponents<ICameraUpdateBehaviour>();

        cursor = GetComponent<ICursorProvider>();

        WorldController wc = worldController_go.GetComponent<WorldController>();

        Vector2 margin = new Vector2(1, 1);
        Vector2 cameraMinExtent = margin;
        Vector2 cameraMaxExtent = new Vector2(wc.Width, wc.Height) - margin;

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
                updateBehaviour.UpdateCamera(constrainedCamera, cursor);
            }
        }
    }
}
