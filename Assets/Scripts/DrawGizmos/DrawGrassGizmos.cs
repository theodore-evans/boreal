using UnityEngine;

public class DrawGrassGizmos : MonoBehaviour
{
    [SerializeField] bool showOpenSet = false;

    private ref Cache<Tile> openSet => ref growGrass.openSet;

    private GrowGrass growGrass = null;

    private void Start()
    {
        growGrass = GetComponent<GrowGrass>();
    }

    private void OnDrawGizmosSelected()
    {
        if (growGrass != null) { 
            if (openSet != null && showOpenSet) {
                foreach (Tile t in openSet) {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(new Vector3(t.X + t.Scale / 2f, t.Y + t.Scale / 2f, -2f), Vector3.one * 0.9f);
                }
            }
        }
    }
}
