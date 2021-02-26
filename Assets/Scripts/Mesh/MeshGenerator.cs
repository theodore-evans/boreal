using UnityEngine;
using System.Collections;

public class MeshGenerator
{
	public Mesh CreateMesh(int bottomLeftX, int bottomLeftY, int width, int height)
	{
		MeshData meshData = new MeshData(width, height);
		int vertexIndex = 0;

		for (int y = bottomLeftY; y < bottomLeftY + height; y++) {
			for (int x = bottomLeftX; x < bottomLeftX + width; x++) {

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
