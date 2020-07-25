using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class NavigationController : MonoBehaviour
{
    public GameObject tileCursor;
    public GameObject circleCursor;
    public GameObject tileInfo;

    Vector3 lastWorldpointUnderMouse;

    float xExtent;
    float yExtent;

    // Start is called before the first frame update
    void Start()
    {
        circleCursor.SetActive(false);

        xExtent = (float)WorldController.Instance.World.Width;
        yExtent = (float)WorldController.Instance.World.Height;

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(currFramePosition);

        if (EventSystem.current.IsPointerOverGameObject() || IsPointerOutOfFrame()) {
            tileCursor.SetActive(false);
            tileInfo.SetActive(false);
        }
        else {
 
            UpdateTileCursor();
            UpdateCameraZoom();
            UpdateCameraDragging();

            lastWorldpointUnderMouse = GetWorldPointUnderMouse();
        }
    }

    void UpdateTileCursor()
    {
        Tile t = GetTileUnderMouse();

        if (t != null) {
            tileCursor.SetActive(true);
            tileInfo.SetActive(true);

            tileInfo.GetComponent<TextMeshProUGUI>().text =
              $"[{t.X}, {t.Y}]\n{t.Type}\nElevation: " + t.Altitude.ToString("F2");

            Vector3 cursorPosition = new Vector3(t.X, t.Y, -10);
            tileCursor.transform.position = cursorPosition;
        }
    }

    void UpdateCameraDragging()
    {
        Vector3 currCameraPosition = Camera.main.transform.position;

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
            Vector3 diff = lastWorldpointUnderMouse - GetWorldPointUnderMouse();

            Vector3 newCameraPosition = new Vector3(
            currCameraPosition.x + diff.x,
            currCameraPosition.y + diff.y,
            currCameraPosition.z);

            Camera.main.transform.position = ConstrainedCameraPosition(newCameraPosition);
        }
    }

    void UpdateCameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") == 0) return;

        float scaleFactor = 1 - Input.GetAxis("Mouse ScrollWheel");
        float oldScale = Camera.main.orthographicSize;
        float newScale = ConstrainedCameraScale(Camera.main.orthographicSize * scaleFactor);
        Camera.main.orthographicSize = newScale;

        //only shift move camera with scale if it is actually scaling 
        if (newScale != oldScale) {
            float scalePercent = scaleFactor;
            float cameraX = Camera.main.transform.position.x;
            float cameraY = Camera.main.transform.position.y;
            float cursorX = GetWorldPointUnderMouse().x;
            float cursorY = GetWorldPointUnderMouse().y;

            Vector3 newCameraPosition = new Vector3(

                (1 - scalePercent) * cursorX + scalePercent * cameraX,
                (1 - scalePercent) * cursorY + scalePercent * cameraY,
                Camera.main.transform.position.z
            );

            //Debug.Log($"xExtent: {xExtent}, yExtent: {yExtent}, maximum scale: {maximumScale}, current aspect ration: {currAspect}, current height: {currCameraHeight}, current width: {currCameraWidth}");
            Camera.main.transform.position = ConstrainedCameraPosition(newCameraPosition);
        }
    }

    Vector3 ConstrainedCameraPosition(Vector3 newCameraPosition)
    {
        float aspect = Camera.main.aspect;
        float currCameraHeight = Camera.main.orthographicSize;
        float currCameraWidth = currCameraHeight * aspect;

        Vector3 constrainedCameraPosition = new Vector3(

            Mathf.Clamp(newCameraPosition.x, currCameraWidth, xExtent - currCameraWidth),
            Mathf.Clamp(newCameraPosition.y, currCameraHeight, yExtent - currCameraHeight),
            newCameraPosition.z
        );

        return constrainedCameraPosition;
    }

    float ConstrainedCameraScale(float newCameraScale)
    {
        float aspect = Camera.main.aspect;
        float maximumScale = Mathf.Min(yExtent, xExtent / aspect) / 2f;

        return Mathf.Clamp(newCameraScale, 3f, Mathf.Min(25f, maximumScale));
    }


    public static Vector3 GetWorldPointUnderMouse()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
    }

    Tile GetTileUnderMouse()
    {
        return WorldController.Instance.GetTileAt(GetWorldPointUnderMouse());
    }

    bool IsPointerOutOfFrame()
    {
        return (0 > Input.mousePosition.x
                || 0 > Input.mousePosition.y
                || Screen.width < Input.mousePosition.x
                || Screen.height < Input.mousePosition.y);
    }
}
