using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour
{
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public Shader meshShader;

	public void DrawMesh(Mesh mesh, Texture2D texture)
	{
		meshFilter.sharedMesh = mesh;
		meshRenderer.material.SetTexture("_Control", texture);
	}

}
