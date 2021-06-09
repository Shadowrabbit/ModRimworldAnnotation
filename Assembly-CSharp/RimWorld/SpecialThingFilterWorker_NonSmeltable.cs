using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D16 RID: 7446
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		// Token: 0x0600A1F9 RID: 41465 RVA: 0x0006BA81 File Offset: 0x00069C81
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		// Token: 0x0600A1FA RID: 41466 RVA: 0x0006BA9C File Offset: 0x00069C9C
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.smeltable || def.MadeFromStuff;
		}

		// Token: 0x0600A1FB RID: 41467 RVA: 0x0006BAAE File Offset: 0x00069CAE
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.smeltable && !def.MadeFromStuff;
		}
	}
}
