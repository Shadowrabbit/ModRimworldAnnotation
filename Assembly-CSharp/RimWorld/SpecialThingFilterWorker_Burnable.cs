using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D18 RID: 7448
	public class SpecialThingFilterWorker_Burnable : SpecialThingFilterWorker
	{
		// Token: 0x0600A201 RID: 41473 RVA: 0x0006BAE1 File Offset: 0x00069CE1
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.BurnableByRecipe;
		}

		// Token: 0x0600A202 RID: 41474 RVA: 0x0006BAF9 File Offset: 0x00069CF9
		public override bool CanEverMatch(ThingDef def)
		{
			return def.burnableByRecipe;
		}

		// Token: 0x0600A203 RID: 41475 RVA: 0x0006BB01 File Offset: 0x00069D01
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
