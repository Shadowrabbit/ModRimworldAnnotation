using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047E RID: 1150
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x06001D08 RID: 7432 RVA: 0x000F2D5C File Offset: 0x000F0F5C
		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000F2E98 File Offset: 0x000F1098
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

		// Token: 0x06001D0A RID: 7434 RVA: 0x000F2ECC File Offset: 0x000F10CC
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

		// Token: 0x06001D0B RID: 7435 RVA: 0x0001A30E File Offset: 0x0001850E
		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000F2F00 File Offset: 0x000F1100
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count);
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040014AD RID: 5293
		private const int MaxGridMeshSize = 15;

		// Token: 0x040014AE RID: 5294
		private const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x040014AF RID: 5295
		private const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x040014B0 RID: 5296
		private const float HumanlikeHeadNarrowWidth = 1.3f;

		// Token: 0x040014B1 RID: 5297
		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		// Token: 0x040014B2 RID: 5298
		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		// Token: 0x040014B3 RID: 5299
		public static readonly GraphicMeshSet humanlikeHairSetAverage = new GraphicMeshSet(1.5f);

		// Token: 0x040014B4 RID: 5300
		public static readonly GraphicMeshSet humanlikeHairSetNarrow = new GraphicMeshSet(1.3f, 1.5f);

		// Token: 0x040014B5 RID: 5301
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x040014B6 RID: 5302
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x040014B7 RID: 5303
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x040014B8 RID: 5304
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x040014B9 RID: 5305
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x040014BA RID: 5306
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x040014BB RID: 5307
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x040014BC RID: 5308
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x040014BD RID: 5309
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x040014BE RID: 5310
		public static readonly Mesh wholeMapPlane;

		// Token: 0x040014BF RID: 5311
		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040014C0 RID: 5312
		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040014C1 RID: 5313
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x040014C2 RID: 5314
		public static readonly Mesh[] pies = new Mesh[361];
	}
}
