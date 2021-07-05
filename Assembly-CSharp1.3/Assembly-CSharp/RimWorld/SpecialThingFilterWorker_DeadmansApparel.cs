using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BA RID: 5306
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x06007EC4 RID: 32452 RVA: 0x002CE828 File Offset: 0x002CCA28
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x06007EC5 RID: 32453 RVA: 0x002CE80E File Offset: 0x002CCA0E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
