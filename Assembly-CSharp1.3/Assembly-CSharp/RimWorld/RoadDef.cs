using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB4 RID: 2740
	public class RoadDef : Def
	{
		// Token: 0x060040FD RID: 16637 RVA: 0x0015E834 File Offset: 0x0015CA34
		public float GetLayerWidth(RoadWorldLayerDef def)
		{
			if (this.cachedLayerWidth == null)
			{
				this.cachedLayerWidth = new float[DefDatabase<RoadWorldLayerDef>.DefCount];
				for (int i = 0; i < DefDatabase<RoadWorldLayerDef>.DefCount; i++)
				{
					RoadWorldLayerDef roadWorldLayerDef = DefDatabase<RoadWorldLayerDef>.AllDefsListForReading[i];
					if (this.worldRenderSteps != null)
					{
						foreach (RoadDef.WorldRenderStep worldRenderStep in this.worldRenderSteps)
						{
							if (worldRenderStep.layer == roadWorldLayerDef)
							{
								this.cachedLayerWidth[(int)roadWorldLayerDef.index] = worldRenderStep.width;
							}
						}
					}
				}
			}
			return this.cachedLayerWidth[(int)def.index];
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0015E8E8 File Offset: 0x0015CAE8
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x0400265A RID: 9818
		public int priority;

		// Token: 0x0400265B RID: 9819
		public bool ancientOnly;

		// Token: 0x0400265C RID: 9820
		public float movementCostMultiplier = 1f;

		// Token: 0x0400265D RID: 9821
		public int tilesPerSegment = 15;

		// Token: 0x0400265E RID: 9822
		public RoadPathingDef pathingMode;

		// Token: 0x0400265F RID: 9823
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x04002660 RID: 9824
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x04002661 RID: 9825
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x04002662 RID: 9826
		public float distortionFrequency = 1f;

		// Token: 0x04002663 RID: 9827
		public float distortionIntensity;

		// Token: 0x04002664 RID: 9828
		[Unsaved(false)]
		private float[] cachedLayerWidth;

		// Token: 0x02002029 RID: 8233
		public class WorldRenderStep
		{
			// Token: 0x04007B47 RID: 31559
			public RoadWorldLayerDef layer;

			// Token: 0x04007B48 RID: 31560
			public float width;
		}
	}
}
