﻿using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A1 RID: 673
	public static class Printer_Mesh
	{
		// Token: 0x0600115C RID: 4444 RVA: 0x000C1880 File Offset: 0x000BFA80
		public static void PrintMesh(SectionLayer layer, Vector3 center, Mesh mesh, Material mat)
		{
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			int vertexCount = mesh.vertexCount;
			Vector3[] vertices = mesh.vertices;
			Color32[] colors = mesh.colors32;
			Vector2[] uv = mesh.uv;
			for (int i = 0; i < vertexCount; i++)
			{
				subMesh.verts.Add(vertices[i] + center);
				if (colors.Length > i)
				{
					subMesh.colors.Add(colors[i]);
				}
				else
				{
					subMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				}
				if (uv.Length > i)
				{
					subMesh.uvs.Add(uv[i]);
				}
				else
				{
					subMesh.uvs.Add(Vector2.zero);
				}
			}
			foreach (int num in mesh.triangles)
			{
				subMesh.tris.Add(count + num);
			}
		}
	}
}
