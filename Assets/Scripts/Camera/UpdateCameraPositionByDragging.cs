using UnityEngine;

public class UpdateCameraPositionByDragging : MonoBehaviour, ICameraUpdateBehaviour
{
    Vector3 oldCursorPosition;

    public void UpdateCamera(ConstrainedCamera camera, ICursorProvider cursor)
    {
        Vector3 oldCameraPosition = camera.Position;

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
            Vector3 diff = oldCursorPosition - cursor.GetPosition();

            if (diff != Vector3.zero) {

                Vector3 newCameraPosition = new Vector3(
                oldCameraPosition.x + diff.x,
                oldCameraPosition.y + diff.y,
                oldCameraPosition.z);

                camera.Position = newCameraPosition;
            }
        }
        oldCursorPosition = cursor.GetPosition();
    }


}
