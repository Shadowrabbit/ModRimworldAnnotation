using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1C RID: 7452
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600A210 RID: 41488 RVA: 0x0006BBA9 File Offset: 0x00069DA9
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && !EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x0600A211 RID: 41489 RVA: 0x000204B9 File Offset: 0x0001E6B9
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
