using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AD RID: 5293
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x06007E95 RID: 32405 RVA: 0x002CE4B0 File Offset: 0x002CC6B0
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return t.def.IsIngestible;
			}
			return compRottable.Stage == RotStage.Fresh;
		}

		// Token: 0x06007E96 RID: 32406 RVA: 0x002CE4E1 File Offset: 0x002CC6E1
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x06007E97 RID: 32407 RVA: 0x002CE4F4 File Offset: 0x002CC6F4
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
