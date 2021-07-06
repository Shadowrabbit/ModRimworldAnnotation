using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D15 RID: 7445
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x0600A1F5 RID: 41461 RVA: 0x0006BA4C File Offset: 0x00069C4C
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x0600A1F6 RID: 41462 RVA: 0x0006BA64 File Offset: 0x00069C64
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x0600A1F7 RID: 41463 RVA: 0x0006BA6C File Offset: 0x00069C6C
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
