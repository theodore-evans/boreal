using UnityEngine;

// Adapter for Camera to constrain the camera view to the world extents and the range of allowed orthographic sizes
// The format is necessary as Camera is a sealed class, so cannot be inherited

public class ConstrainedCamera
{
    private Camera camera;
    private Vector2 minExtents;
    private Vector2 maxExtents;

    private float minScale;
    private float maxScale;

    public ConstrainedCamera(Camera camera, Vector2 minExtents, Vector2 maxExtents, float minScale, float maxScale)
    {
        this.camera = camera;
        this.minExtents = minExtents;
        this.maxExtents = maxExtents;
        this.minScale = minScale;
        this.maxScale = maxScale;

        Position = camera.transform.position;
        OrthographicSize = camera.orthographicSize;
    }

    public Vector3 Position
    {
        get => camera.transform.position;
        set {
            Vector3 newCameraPosition = value;

            float aspect = camera.aspect;
            float currCameraHeight = camera.orthographicSize;
            float currCameraWidth = currCameraHeight * aspect;

            Vector3 constrainedCameraPosition = new Vector3(

                Mathf.Clamp(newCameraPosition.x, minExtents.x + currCameraWidth, maxExtents.x - currCameraWidth),
                Mathf.Clamp(newCameraPosition.y, minExtents.y + currCameraHeight, maxExtents.y - currCameraHeight),
                newCameraPosition.z
            );

            camera.transform.position = constrainedCameraPosition;
        }
    }

    public float OrthographicSize
    {
        get => camera.orthographicSize;
        set {
            float newCameraScale = value;

            float aspect = camera.aspect;
            float fullScale = Mathf.Min(maxExtents.y - minExtents.y, (maxExtents.x - minExtents.y) / aspect) / 2f;

            camera.orthographicSize = Mathf.Clamp(newCameraScale, Mathf.Min(minScale, fullScale), Mathf.Min(maxScale, fullScale));
            Position = camera.transform.position;
        }
    }
}
