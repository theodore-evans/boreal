using UnityEngine;

public class DrawWaterGizmos : MonoBehaviour
{
    [SerializeField] bool showWaterFlow = true;
    [SerializeField] bool showOpenSet = false;
    [SerializeField] [Range(0, 1)] float showFlowMin = 0.2f;
    [SerializeField] [Range(0, 1)] float showFlowMax = 1f;

    private ref VisitationMap<Tile> moistureMap => ref simulateWaterFlow.moistureMap;
    private ref Cache<Tile> openSet => ref simulateWaterFlow.openSet;

    private SimulateWaterFlow simulateWaterFlow = null;

    private void Start()
    {
        simulateWaterFlow = GetComponent<SimulateWaterFlow>();
    }

    private void OnDrawGizmosSelected()
    {
        if (simulateWaterFlow != null) {
            if (moistureMap != null) {
                foreach (Tile t in moistureMap) {
                    if (moistureMap[t] > showFlowMin && moistureMap[t] <= showFlowMax) {
                        Color color = Color.blue;
                        if (showWaterFlow) {
                            color.a = Mathf.Lerp(0, 1, Mathf.Clamp01((moistureMap[t] - showFlowMin) / showFlowMax));
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
