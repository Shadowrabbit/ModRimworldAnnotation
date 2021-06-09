using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1F RID: 7455
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600A219 RID: 41497 RVA: 0x002F4BA8 File Offset: 0x002F2DA8
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0600A21A RID: 41498 RVA: 0x0006BC10 File Offset: 0x00069E10
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
