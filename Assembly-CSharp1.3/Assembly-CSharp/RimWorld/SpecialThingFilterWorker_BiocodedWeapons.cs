using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B5 RID: 5301
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06007EB5 RID: 32437 RVA: 0x002CE740 File Offset: 0x002CC940
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x002CE757 File Offset: 0x002CC957
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodable));
		}
	}
}
