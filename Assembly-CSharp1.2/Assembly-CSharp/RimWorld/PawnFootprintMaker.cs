using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B4 RID: 5044
	public class PawnFootprintMaker
	{
		// Token: 0x06006D6B RID: 28011 RVA: 0x0004A5D1 File Offset: 0x000487D1
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006D6C RID: 28012 RVA: 0x00218974 File Offset: 0x00216B74
		public void FootprintMakerTick()
		{
			if (!this.pawn.RaceProps.makesFootprints)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain == null || !terrain.takeSplashes)
				{
					return;
				}
			}
			if ((this.pawn.Drawer.DrawPos - this.lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.39942405f)
			{
				this.TryPlaceFootprint();
			}
		}

		// Token: 0x06006D6D RID: 28013 RVA: 0x002189E8 File Offset: 0x00216BE8
		private void TryPlaceFootprint()
		{
			Vector3 drawPos = this.pawn.Drawer.DrawPos;
			Vector3 normalized = (drawPos - this.lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = (float)(this.lastFootprintRight ? 90 : -90);
			Vector3 b = normalized.RotatedBy(angle) * 0.17f * Mathf.Sqrt(this.pawn.BodySize);
			Vector3 vector = drawPos + PawnFootprintMaker.FootprintOffset + b;
			IntVec3 c = vector.ToIntVec3();
			if (c.InBounds(this.pawn.Map))
			{
				TerrainDef terrain = c.GetTerrain(this.pawn.Map);
				if (terrain != null)
				{
					if (terrain.takeSplashes)
					{
						MoteMaker.MakeWaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						MoteMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}

		// Token: 0x04004854 RID: 18516
		private Pawn pawn;

		// Token: 0x04004855 RID: 18517
		private Vector3 lastFootprintPlacePos;

		// Token: 0x04004856 RID: 18518
		private bool lastFootprintRight;

		// Token: 0x04004857 RID: 18519
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x04004858 RID: 18520
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x04004859 RID: 18521
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x0400485A RID: 18522
		private const float FootprintSplashSize = 2f;
	}
}
