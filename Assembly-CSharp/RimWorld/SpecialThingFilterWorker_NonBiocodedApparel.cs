using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1E RID: 7454
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x0600A216 RID: 41494 RVA: 0x0006BBF6 File Offset: 0x00069DF6
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !EquipmentUtility.IsBiocoded(t);
		}

		// Token: 0x0600A217 RID: 41495 RVA: 0x0001ECA9 File Offset: 0x0001CEA9
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
