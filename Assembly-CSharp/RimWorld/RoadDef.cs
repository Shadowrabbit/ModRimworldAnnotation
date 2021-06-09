using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD3 RID: 4051
	public class RoadDef : Def
	{
		// Token: 0x0600587A RID: 22650 RVA: 0x001D0278 File Offset: 0x001CE478
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

		// Token: 0x0600587B RID: 22651 RVA: 0x0003D7F7 File Offset: 0x0003B9F7
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x04003A9B RID: 15003
		public int priority;

		// Token: 0x04003A9C RID: 15004
		public bool ancientOnly;

		// Token: 0x04003A9D RID: 15005
		public float movementCostMultiplier = 1f;

		// Token: 0x04003A9E RID: 15006
		public int tilesPerSegment = 15;

		// Token: 0x04003A9F RID: 15007
		public RoadPathingDef pathingMode;

		// Token: 0x04003AA0 RID: 15008
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x04003AA1 RID: 15009
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x04003AA2 RID: 15010
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x04003AA3 RID: 15011
		public float distortionFrequency = 1f;

		// Token: 0x04003AA4 RID: 15012
		public float distortionIntensity;

		// Token: 0x04003AA5 RID: 15013
		[Unsaved(false)]
		private float[] cachedLayerWidth;

		// Token: 0x02000FD4 RID: 4052
		public class WorldRenderStep
		{
			// Token: 0x04003AA6 RID: 15014
			public RoadWorldLayerDef layer;

			// Token: 0x04003AA7 RID: 15015
			public float width;
		}
	}
}
