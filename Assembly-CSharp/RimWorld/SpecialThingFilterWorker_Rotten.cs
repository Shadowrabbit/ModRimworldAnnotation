using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D12 RID: 7442
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x0600A1EA RID: 41450 RVA: 0x002F4A90 File Offset: 0x002F2C90
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x0600A1EB RID: 41451 RVA: 0x002F4AC0 File Offset: 0x002F2CC0
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
