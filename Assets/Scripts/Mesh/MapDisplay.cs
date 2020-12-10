using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour
{
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public void DrawMesh(Mesh mesh, Texture2D texture)
	{
		meshFilter.sharedMesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = texture;
	}

}
