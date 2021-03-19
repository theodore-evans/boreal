using UnityEngine;

public class MouseCursorProvider : MonoBehaviour, ICursorProvider
{
    [SerializeField] Camera currCamera = null;
   
    public bool IsPointerOutOfFrame => (Input.mousePosition.x < 0
                                     || Input.mousePosition.x > Screen.width
                                     || Input.mousePosition.y < 0
                                     || Input.mousePosition.y > Screen.height);

    public Vector3 GetPosition()
    {
        return currCamera.ScreenToWorldPoint(Input.mousePosition);
    }

}
