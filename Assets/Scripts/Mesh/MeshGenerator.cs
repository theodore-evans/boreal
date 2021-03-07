using UnityEngine;
using System.Collections;

public class MeshGenerator: MonoBehaviour
{
	[SerializeField] int levelOfDetail = 0; //TODO implement for non-square maps

	public Mesh CreateMesh(Vector3 bottomLeftCorner, int width, int height)
	{
		int meshSimplificationIncrement = Mathf.Max(width, (int)Mathf.Pow(2, levelOfDetail));
		int verticesPerLine = width / meshSimplificationIncrement + 1;

		MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);

		int vertexIndex = 0;

		float bottomLeftX = bottomLeftCorner.x;
		float bottomLeftY = bottomLeftCorner.y;
		float zPos = bottomLeftCorner.z;

		for (int y = 0; y <= height; y += meshSimplificationIncrement) {
			for (int x = 0; x <= width; x += meshSimplificationIncrement) {
				meshData.vertices[vertexIndex] = new Vector3(bottomLeftX + x, bottomLeftY + y, zPos);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
					meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex + 1, vertexIndex);
				}

				vertexIndex++;
			}
		}

		return meshData.CreateMesh();

	}
}
