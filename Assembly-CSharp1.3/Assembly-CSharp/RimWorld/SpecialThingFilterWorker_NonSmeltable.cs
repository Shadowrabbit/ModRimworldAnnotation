using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B0 RID: 5296
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		// Token: 0x06007EA1 RID: 32417 RVA: 0x002CE58C File Offset: 0x002CC78C
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x002CE5A7 File Offset: 0x002CC7A7
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.smeltable || def.MadeFromStuff;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x002CE5B9 File Offset: 0x002CC7B9
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.smeltable && !def.MadeFromStuff;
		}
	}
}
