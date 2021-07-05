using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AF RID: 5295
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x06007E9D RID: 32413 RVA: 0x002CE557 File Offset: 0x002CC757
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x06007E9E RID: 32414 RVA: 0x002CE56F File Offset: 0x002CC76F
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x002CE577 File Offset: 0x002CC777
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
