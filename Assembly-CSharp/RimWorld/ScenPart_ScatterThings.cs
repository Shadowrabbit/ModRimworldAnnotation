using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161D RID: 5661
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06007B12 RID: 31506
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x06007B13 RID: 31507 RVA: 0x0024FF88 File Offset: 0x0024E188
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
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4)
			}.Generate(map, default(GenStepParams));
		}
	}
}
