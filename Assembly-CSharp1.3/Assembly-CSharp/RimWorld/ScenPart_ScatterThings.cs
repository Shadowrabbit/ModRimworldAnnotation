using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001016 RID: 4118
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x0600611A RID: 24858
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x0600611B RID: 24859 RVA: 0x0020FFB8 File Offset: 0x0020E1B8
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			new GenStep_ScatterThings
			{
				nearPlayerStart = this.NearPlayerStart,
				allowFoggedPositions = !this.NearPlayerStart,
				thingDef = this.thingDef,
				stuff = this.stuff,
				count = this.count,
				spotMustBeStandable = true,
				minSpacing = 5f,
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4),
				allowRoofed = this.allowRoofed
			}.Generate(map, default(GenStepParams));
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x00210056 File Offset: 0x0020E256
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.allowRoofed, "allowRoofed", false, false);
		}

		// Token: 0x04003767 RID: 14183
		public bool allowRoofed = true;
	}
}
