using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D20 RID: 7456
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600A21C RID: 41500 RVA: 0x002F4BCC File Offset: 0x002F2DCC
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x0600A21D RID: 41501 RVA: 0x0006BC10 File Offset: 0x00069E10
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
