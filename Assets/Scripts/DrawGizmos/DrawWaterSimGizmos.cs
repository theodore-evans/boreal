using UnityEngine;

public class DrawWaterSimGizmos : MonoBehaviour
{
    [SerializeField] bool showWaterFlow = true;
    [SerializeField] bool showOpenSet = false;
    [SerializeField] [Range(0, 100)] int showFlowMin = 2;
    [SerializeField] [Range(0, 100)] int showFlowMax = 10;

    private ref VisitationMap<Tile> visitedSet => ref simulateWaterFlow.visitedSet;
    private ref Cache<Tile> openSet => ref simulateWaterFlow.openSet;

    private SimulateWaterFlow simulateWaterFlow = null;

    private void Start()
    {
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
    }

    private void OnDrawGizmosSelected()
    {
        if (simulateWaterFlow != null) {
            if (visitedSet != null) {
                foreach (Tile t in visitedSet) {
                    if (visitedSet[t] > showFlowMin) {
                        Color color = Color.blue;
                        if (showWaterFlow) {
                            color.a = Mathf.Lerp(0, 1, Mathf.Clamp01((visitedSet[t] - showFlowMin) / showFlowMax));
                        }
                        Gizmos.color = color;
                        Gizmos.DrawCube(new Vector3(t.X + t.Scale / 2f, t.Y + t.Scale / 2f, -2f), Vector3.one * 0.9f);
                    }
                }
            }

            if (openSet != null && showOpenSet) {
                foreach (Tile t in openSet) {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(new Vector3(t.X + t.Scale / 2f, t.Y + t.Scale / 2f, -2f), Vector3.one * 0.9f);
                }
            }
        }
    }
}
