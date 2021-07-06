using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D19 RID: 7449
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		// Token: 0x0600A205 RID: 41477 RVA: 0x0006BB16 File Offset: 0x00069D16
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x0600A206 RID: 41478 RVA: 0x0006BB31 File Offset: 0x00069D31
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		// Token: 0x0600A207 RID: 41479 RVA: 0x0006BB43 File Offset: 0x00069D43
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
