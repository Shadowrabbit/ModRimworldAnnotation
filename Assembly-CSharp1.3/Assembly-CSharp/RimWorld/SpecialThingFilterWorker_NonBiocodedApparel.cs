using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B8 RID: 5304
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x06007EBE RID: 32446 RVA: 0x002CE7C8 File Offset: 0x002CC9C8
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x002CE7E2 File Offset: 0x002CC9E2
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
