using UnityEngine;

public class MouseCursorProvider : MonoBehaviour, ICursorProvider
{
    Camera currentCamera;
   
    public bool IsPointerOutOfFrame => (Input.mousePosition.x < 0
                                     || Input.mousePosition.x > Screen.width
                                     || Input.mousePosition.y < 0
                                     || Input.mousePosition.y > Screen.height);

    public Vector3 GetPosition()
    {
        return currentCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetCamera(ref Camera camera)
    {
        currentCamera = camera;
    }
}
