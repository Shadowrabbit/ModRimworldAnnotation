using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030A RID: 778
	public static class MeshMakerPlanes
	{
		// Token: 0x0600166E RID: 5742 RVA: 0x00082A93 File Offset: 0x00080C93
		public static Mesh NewPlaneMesh(float size)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, false);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00082A9C File Offset: 0x00080C9C
		public static Mesh NewPlaneMesh(float size, bool flipped)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, flipped, false);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00082AA6 File Offset: 0x00080CA6
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, false);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00082AB7 File Offset: 0x00080CB7
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00082AC8 File Offset: 0x00080CC8
		public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
			array[1] = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
			array[2] = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
			array[3] = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
			if (backLift)
			{
				array[1].y = 0.002027027f;
				array[2].y = 0.002027027f;
				array[3].y = 0.00081081083f;
			}
			if (twist)
			{
				array[0].y = 0.0010135135f;
				array[1].y = 0.00050675677f;
				array[2].y = 0f;
				array[3].y = 0.00050675677f;
			}
			if (!flipped)
			{
				array2[0] = new Vector2(0f, 0f);
				array2[1] = new Vector2(0f, 1f);
				array2[2] = new Vector2(1f, 1f);
				array2[3] = new Vector2(1f, 0f);
			}
			else
			{
				array2[0] = new Vector2(1f, 0f);
				array2[1] = new Vector2(1f, 1f);
				array2[2] = new Vector2(0f, 1f);
				array2[3] = new Vector2(0f, 0f);
			}
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			Mesh mesh = new Mesh();
			mesh.name = "NewPlaneMesh()";
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.SetTriangles(array3, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00082D0C File Offset: 0x00080F0C
		public static Mesh NewWholeMapPlane()
		{
			Mesh mesh = MeshMakerPlanes.NewPlaneMesh(2000f, false, false);
			Vector2[] array = new Vector2[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = mesh.uv[i] * 200f;
			}
			mesh.uv = array;
			return mesh;
		}

		// Token: 0x04000F91 RID: 3985
		private const float BackLiftAmount = 0.002027027f;

		// Token: 0x04000F92 RID: 3986
		private const float TwistAmount = 0.0010135135f;
	}
}
