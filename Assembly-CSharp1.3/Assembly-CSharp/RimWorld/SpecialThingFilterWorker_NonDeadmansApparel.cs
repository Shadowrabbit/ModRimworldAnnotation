using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B9 RID: 5305
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x06007EC1 RID: 32449 RVA: 0x002CE7EC File Offset: 0x002CC9EC
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x002CE80E File Offset: 0x002CCA0E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
