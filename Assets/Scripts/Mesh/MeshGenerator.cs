using UnityEngine;
using System.Collections;

public static class MeshGenerator
{
	public static Mesh GenerateTerrainMesh(int width, int height)
	{
		MeshData meshData = new MeshData(width, height);
		int vertexIndex = 0;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				meshData.vertices[vertexIndex] = new Vector3(x, y, 0);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);
					meshData.AddTriangle(vertexIndex + width + 1, vertexIndex + 1, vertexIndex);
				}

				vertexIndex++;
			}
		}

		return meshData.CreateMesh();

	}
}
