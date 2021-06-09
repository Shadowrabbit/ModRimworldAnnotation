using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1D RID: 7453
	public class SpecialThingFilterWorker_BiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600A213 RID: 41491 RVA: 0x0006BBC3 File Offset: 0x00069DC3
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x0600A214 RID: 41492 RVA: 0x0006BBDA File Offset: 0x00069DDA
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.HasComp(typeof(CompBiocodableApparel));
		}
	}
}
