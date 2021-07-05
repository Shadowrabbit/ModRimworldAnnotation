using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AC RID: 5292
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x06007E92 RID: 32402 RVA: 0x002CE45C File Offset: 0x002CC65C
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x06007E93 RID: 32403 RVA: 0x002CE48C File Offset: 0x002CC68C
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
