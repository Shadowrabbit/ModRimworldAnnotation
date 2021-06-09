using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	public static class MeshMakerShadows
	{
		// Token: 0x06001CFE RID: 7422 RVA: 0x000F2928 File Offset: 0x000F0B28
		public static Mesh NewShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			Color32 item = new Color32(byte.MaxValue, 0, 0, (byte)(255f * tallness));
			float num = baseWidth / 2f;
			float num2 = baseHeight / 2f;
			MeshMakerShadows.vertsList.Clear();
			MeshMakerShadows.colorsList.Clear();
			MeshMakerShadows.trianglesList.Clear();
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, num2));
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, num2));
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.colorsList.Add(MeshMakerShadows.LowVertexColor);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(1);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(3);
			int count = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(count);
			MeshMakerShadows.trianglesList.Add(count + 1);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(count + 1);
			MeshMakerShadows.trianglesList.Add(1);
			int count2 = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(2);
			MeshMakerShadows.trianglesList.Add(count2);
			MeshMakerShadows.trianglesList.Add(count2 + 1);
			MeshMakerShadows.trianglesList.Add(count2 + 1);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(2);
			int count3 = MeshMakerShadows.vertsList.Count;
			MeshMakerShadows.vertsList.Add(new Vector3(-num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.vertsList.Add(new Vector3(num, 0f, -num2));
			MeshMakerShadows.colorsList.Add(item);
			MeshMakerShadows.trianglesList.Add(0);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(count3);
			MeshMakerShadows.trianglesList.Add(3);
			MeshMakerShadows.trianglesList.Add(count3 + 1);
			MeshMakerShadows.trianglesList.Add(count3);
			return new Mesh
			{
				name = "NewShadowMesh()",
				vertices = MeshMakerShadows.vertsList.ToArray(),
				colors32 = MeshMakerShadows.colorsList.ToArray(),
				triangles = MeshMakerShadows.trianglesList.ToArray()
			};
		}

		// Token: 0x040014A7 RID: 5287
		private static List<Vector3> vertsList = new List<Vector3>();

		// Token: 0x040014A8 RID: 5288
		private static List<Color32> colorsList = new List<Color32>();

		// Token: 0x040014A9 RID: 5289
		private static List<int> trianglesList = new List<int>();

		// Token: 0x040014AA RID: 5290
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);
	}
}
