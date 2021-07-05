using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001492 RID: 5266
	public static class MeshUtility
	{
		// Token: 0x06007DDF RID: 32223 RVA: 0x002C96B0 File Offset: 0x002C78B0
		public static void RemoveVertices(List<Vector3> verts, List<TriangleIndices> tris, Predicate<Vector3> predicate)
		{
			int i = 0;
			int count = tris.Count;
			while (i < count)
			{
				TriangleIndices triangleIndices = tris[i];
				if (predicate(verts[triangleIndices.v1]) || predicate(verts[triangleIndices.v2]) || predicate(verts[triangleIndices.v3]))
				{
					tris[i] = new TriangleIndices(-1, -1, -1);
				}
				i++;
			}
			tris.RemoveAll((TriangleIndices x) => x.v1 == -1);
			MeshUtility.RemoveUnusedVertices(verts, tris);
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x002C9750 File Offset: 0x002C7950
		public static void RemoveUnusedVertices(List<Vector3> verts, List<TriangleIndices> tris)
		{
			MeshUtility.vertIsUsed.Clear();
			int i = 0;
			int count = verts.Count;
			while (i < count)
			{
				MeshUtility.vertIsUsed.Add(false);
				i++;
			}
			int j = 0;
			int count2 = tris.Count;
			while (j < count2)
			{
				TriangleIndices triangleIndices = tris[j];
				MeshUtility.vertIsUsed[triangleIndices.v1] = true;
				MeshUtility.vertIsUsed[triangleIndices.v2] = true;
				MeshUtility.vertIsUsed[triangleIndices.v3] = true;
				j++;
			}
			int num = 0;
			MeshUtility.offsets.Clear();
			int k = 0;
			int count3 = verts.Count;
			while (k < count3)
			{
				if (!MeshUtility.vertIsUsed[k])
				{
					num++;
				}
				MeshUtility.offsets.Add(num);
				k++;
			}
			int l = 0;
			int count4 = tris.Count;
			while (l < count4)
			{
				TriangleIndices triangleIndices2 = tris[l];
				tris[l] = new TriangleIndices(triangleIndices2.v1 - MeshUtility.offsets[triangleIndices2.v1], triangleIndices2.v2 - MeshUtility.offsets[triangleIndices2.v2], triangleIndices2.v3 - MeshUtility.offsets[triangleIndices2.v3]);
				l++;
			}
			verts.RemoveAll((Vector3 elem, int index) => !MeshUtility.vertIsUsed[index]);
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x002C98BB File Offset: 0x002C7ABB
		public static bool Visible(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			return viewAngle >= 180f || Vector3.Angle(viewCenter * radius, point) <= viewAngle;
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x002C98DC File Offset: 0x002C7ADC
		public static bool VisibleForWorldgen(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			if (viewAngle >= 180f)
			{
				return true;
			}
			float num = Vector3.Angle(viewCenter * radius, point) + -1E-05f;
			if (Mathf.Abs(num - viewAngle) < 1E-06f)
			{
				Log.Warning(string.Format("Angle difference {0} is within epsilon; recommend adjusting visibility tweak", num - viewAngle));
			}
			return num <= viewAngle;
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x002C9934 File Offset: 0x002C7B34
		public static Color32 MutateAlpha(this Color32 input, byte newAlpha)
		{
			input.a = newAlpha;
			return input;
		}

		// Token: 0x04004E78 RID: 20088
		private static List<int> offsets = new List<int>();

		// Token: 0x04004E79 RID: 20089
		private static List<bool> vertIsUsed = new List<bool>();
	}
}
