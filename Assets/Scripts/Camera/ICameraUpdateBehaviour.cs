// interface for all behaviours that update the camera every frame (called by CameraController)

public interface ICameraUpdateBehaviour
{
    void UpdateCamera(ref ConstrainedCamera constrainedCamera, ref ICursorProvider cursor);
}