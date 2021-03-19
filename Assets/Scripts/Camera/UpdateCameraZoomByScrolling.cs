using UnityEngine;

public class UpdateCameraZoomByScrolling : MonoBehaviour, ICameraUpdateBehaviour
{
    public void UpdateCamera(ref ConstrainedCamera camera, ref ICursorProvider cursor)
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {

            float scaleFactor = Input.GetAxis("Mouse ScrollWheel");
            float oldScale = camera.OrthographicSize;

            camera.OrthographicSize = oldScale * (1 - scaleFactor);

            camera.Position = new Vector3(scaleFactor * cursor.GetPosition().x + (1 - scaleFactor) * camera.Position.x,
                                          scaleFactor * cursor.GetPosition().y + (1 - scaleFactor) * camera.Position.y,
                                          camera.Position.z);
        }
    }
}
