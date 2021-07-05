using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200177E RID: 6014
	public static class IcosahedronGenerator
	{
		// Token: 0x06008AC1 RID: 35521 RVA: 0x0031C828 File Offset: 0x0031AA28
		public static void GenerateIcosahedron(List<Vector3> outVerts, List<TriangleIndices> outTris, float radius, Vector3 viewCenter, float viewAngle)
		{
			float num = (1f + Mathf.Sqrt(5f)) / 2f;
			outVerts.Clear();
			outVerts.Add(new Vector3(-1f, num, 0f).normalized * radius);
			outVerts.Add(new Vector3(1f, num, 0f).normalized * radius);
			outVerts.Add(new Vector3(-1f, -num, 0f).normalized * radius);
			outVerts.Add(new Vector3(1f, -num, 0f).normalized * radius);
			outVerts.Add(new Vector3(0f, -1f, num).normalized * radius);
			outVerts.Add(new Vector3(0f, 1f, num).normalized * radius);
			outVerts.Add(new Vector3(0f, -1f, -num).normalized * radius);
			outVerts.Add(new Vector3(0f, 1f, -num).normalized * radius);
			outVerts.Add(new Vector3(num, 0f, -1f).normalized * radius);
			outVerts.Add(new Vector3(num, 0f, 1f).normalized * radius);
			outVerts.Add(new Vector3(-num, 0f, -1f).normalized * radius);
			outVerts.Add(new Vector3(-num, 0f, 1f).normalized * radius);
			outTris.Clear();
			int i = 0;
			int num2 = IcosahedronGenerator.IcosahedronTris.Length;
			while (i < num2)
			{
				TriangleIndices triangleIndices = IcosahedronGenerator.IcosahedronTris[i];
				if (IcosahedronGenerator.IcosahedronFaceNeeded(triangleIndices.v1, triangleIndices.v2, triangleIndices.v3, outVerts, radius, viewCenter, viewAngle))
				{
					outTris.Add(triangleIndices);
				}
				i++;
			}
			MeshUtility.RemoveUnusedVertices(outVerts, outTris);
		}

		// Token: 0x06008AC2 RID: 35522 RVA: 0x0031CA60 File Offset: 0x0031AC60
		private static bool IcosahedronFaceNeeded(int v1, int v2, int v3, List<Vector3> verts, float radius, Vector3 viewCenter, float viewAngle)
		{
			viewAngle += 18f;
			return MeshUtility.Visible(verts[v1], radius, viewCenter, viewAngle) || MeshUtility.Visible(verts[v2], radius, viewCenter, viewAngle) || MeshUtility.Visible(verts[v3], radius, viewCenter, viewAngle);
		}

		// Token: 0x04005859 RID: 22617
		private static readonly TriangleIndices[] IcosahedronTris = new TriangleIndices[]
		{
			new TriangleIndices(0, 11, 5),
			new TriangleIndices(0, 5, 1),
			new TriangleIndices(0, 1, 7),
			new TriangleIndices(0, 7, 10),
			new TriangleIndices(0, 10, 11),
			new TriangleIndices(1, 5, 9),
			new TriangleIndices(5, 11, 4),
			new TriangleIndices(11, 10, 2),
			new TriangleIndices(10, 7, 6),
			new TriangleIndices(7, 1, 8),
			new TriangleIndices(3, 9, 4),
			new TriangleIndices(3, 4, 2),
			new TriangleIndices(3, 2, 6),
			new TriangleIndices(3, 6, 8),
			new TriangleIndices(3, 8, 9),
			new TriangleIndices(4, 9, 5),
			new TriangleIndices(2, 4, 11),
			new TriangleIndices(6, 2, 10),
			new TriangleIndices(8, 6, 7),
			new TriangleIndices(9, 8, 1)
		};
	}
}
