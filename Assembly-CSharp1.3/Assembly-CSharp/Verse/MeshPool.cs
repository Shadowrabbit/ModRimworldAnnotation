using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030F RID: 783
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x06001683 RID: 5763 RVA: 0x000832A4 File Offset: 0x000814A4
		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00083440 File Offset: 0x00081640
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00083474 File Offset: 0x00081674
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000834A7 File Offset: 0x000816A7
		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000834D8 File Offset: 0x000816D8
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count);
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000F9A RID: 3994
		private const int MaxGridMeshSize = 15;

		// Token: 0x04000F9B RID: 3995
		public const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x04000F9C RID: 3996
		public const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x04000F9D RID: 3997
		public const float HumanlikeHeadNarrowWidth = 1.3f;

		// Token: 0x04000F9E RID: 3998
		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		// Token: 0x04000F9F RID: 3999
		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		// Token: 0x04000FA0 RID: 4000
		public static readonly GraphicMeshSet humanlikeHairSetAverage = new GraphicMeshSet(1.5f);

		// Token: 0x04000FA1 RID: 4001
		public static readonly GraphicMeshSet humanlikeHairSetNarrow = new GraphicMeshSet(1.3f, 1.5f);

		// Token: 0x04000FA2 RID: 4002
		public static readonly GraphicMeshSet humanlikeBodySet_Male = new GraphicMeshSet(1.3f);

		// Token: 0x04000FA3 RID: 4003
		public static readonly GraphicMeshSet humanlikeBodySet_Female = new GraphicMeshSet(1.3f, 1.4f);

		// Token: 0x04000FA4 RID: 4004
		public static readonly GraphicMeshSet humanlikeBodySet_Hulk = new GraphicMeshSet(1.5f, 1.65f);

		// Token: 0x04000FA5 RID: 4005
		public static readonly GraphicMeshSet humanlikeBodySet_Fat = new GraphicMeshSet(1.6f, 1.4f);

		// Token: 0x04000FA6 RID: 4006
		public static readonly GraphicMeshSet humanlikeBodySet_Thin = new GraphicMeshSet(1.2f, 1.4f);

		// Token: 0x04000FA7 RID: 4007
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x04000FA8 RID: 4008
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x04000FA9 RID: 4009
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x04000FAA RID: 4010
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x04000FAB RID: 4011
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x04000FAC RID: 4012
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x04000FAD RID: 4013
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x04000FAE RID: 4014
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x04000FAF RID: 4015
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x04000FB0 RID: 4016
		public static readonly Mesh wholeMapPlane;

		// Token: 0x04000FB1 RID: 4017
		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04000FB2 RID: 4018
		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x04000FB3 RID: 4019
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x04000FB4 RID: 4020
		public static readonly Mesh[] pies = new Mesh[361];
	}
}
