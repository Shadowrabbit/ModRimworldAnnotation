using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1B RID: 7451
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600A20D RID: 41485 RVA: 0x0006BB76 File Offset: 0x00069D76
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x0600A20E RID: 41486 RVA: 0x0006BB8D File Offset: 0x00069D8D
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodableWeapon));
		}
	}
}
