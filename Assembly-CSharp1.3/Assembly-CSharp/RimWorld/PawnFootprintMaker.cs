using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8D RID: 3469
	public class PawnFootprintMaker
	{
		// Token: 0x06005077 RID: 20599 RVA: 0x001AE31B File Offset: 0x001AC51B
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x001AE32C File Offset: 0x001AC52C
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

		// Token: 0x06005079 RID: 20601 RVA: 0x001AE3A0 File Offset: 0x001AC5A0
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
						FleckMaker.WaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						FleckMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}

		// Token: 0x04002FED RID: 12269
		private Pawn pawn;

		// Token: 0x04002FEE RID: 12270
		private Vector3 lastFootprintPlacePos;

		// Token: 0x04002FEF RID: 12271
		private bool lastFootprintRight;

		// Token: 0x04002FF0 RID: 12272
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x04002FF1 RID: 12273
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x04002FF2 RID: 12274
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x04002FF3 RID: 12275
		private const float FootprintSplashSize = 2f;
	}
}
