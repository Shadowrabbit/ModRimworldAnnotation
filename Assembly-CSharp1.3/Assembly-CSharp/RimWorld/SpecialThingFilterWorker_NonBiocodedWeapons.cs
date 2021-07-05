using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B6 RID: 5302
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06007EB8 RID: 32440 RVA: 0x002CE773 File Offset: 0x002CC973
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && !CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x002CE78D File Offset: 0x002CC98D
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
