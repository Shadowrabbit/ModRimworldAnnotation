using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DD RID: 477
	public static class Printer_Plane
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004CC8C File Offset: 0x0004AE8C
		public static void GetUVs(Rect rect, out Vector2 uv1, out Vector2 uv2, out Vector2 uv3, out Vector2 uv4, bool flipUv)
		{
			if (flipUv)
			{
				uv1 = new Vector2(rect.xMax, rect.yMin);
				uv2 = new Vector2(rect.xMax, rect.yMax);
				uv3 = new Vector2(rect.xMin, rect.yMax);
				uv4 = new Vector2(rect.xMin, rect.yMin);
				return;
			}
			uv1 = new Vector2(rect.xMin, rect.yMin);
			uv2 = new Vector2(rect.xMin, rect.yMax);
			uv3 = new Vector2(rect.xMax, rect.yMax);
			uv4 = new Vector2(rect.xMax, rect.yMin);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004CD68 File Offset: 0x0004AF68
		public static void PrintPlane(SectionLayer layer, Vector3 center, Vector2 size, Material mat, float rot = 0f, bool flipUv = false, Vector2[] uvs = null, Color32[] colors = null, float topVerticesAltitudeBias = 0.01f, float uvzPayload = 0f)
		{
			if (colors == null)
			{
				colors = Printer_Plane.defaultColors;
			}
			if (uvs == null)
			{
				if (!flipUv)
				{
					uvs = Printer_Plane.defaultUvs;
				}
				else
				{
					uvs = Printer_Plane.defaultUvsFlipped;
				}
			}
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3(-0.5f * size.x, 0f, -0.5f * size.y));
			subMesh.verts.Add(new Vector3(-0.5f * size.x, topVerticesAltitudeBias, 0.5f * size.y));
			subMesh.verts.Add(new Vector3(0.5f * size.x, topVerticesAltitudeBias, 0.5f * size.y));
			subMesh.verts.Add(new Vector3(0.5f * size.x, 0f, -0.5f * size.y));
			if (rot != 0f)
			{
				float num = rot * 0.017453292f;
				num *= -1f;
				for (int i = 0; i < 4; i++)
				{
					float x = subMesh.verts[count + i].x;
					float z = subMesh.verts[count + i].z;
					float num2 = Mathf.Cos(num);
					float num3 = Mathf.Sin(num);
					float x2 = x * num2 - z * num3;
					float z2 = x * num3 + z * num2;
					subMesh.verts[count + i] = new Vector3(x2, subMesh.verts[count + i].y, z2);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				List<Vector3> verts = subMesh.verts;
				int index = count + j;
				verts[index] += center;
				subMesh.uvs.Add(new Vector3(uvs[j].x, uvs[j].y, uvzPayload));
				subMesh.colors.Add(colors[j]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x04000B2A RID: 2858
		private static Color32[] defaultColors = new Color32[]
		{
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		};

		// Token: 0x04000B2B RID: 2859
		private static Vector2[] defaultUvs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x04000B2C RID: 2860
		private static Vector2[] defaultUvsFlipped = new Vector2[]
		{
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f)
		};
	}
}
