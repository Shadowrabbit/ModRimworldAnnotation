using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D13 RID: 7443
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x0600A1ED RID: 41453 RVA: 0x002F4AE4 File Offset: 0x002F2CE4
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return t.def.IsIngestible;
			}
			return compRottable.Stage == RotStage.Fresh;
		}

		// Token: 0x0600A1EE RID: 41454 RVA: 0x0006BA06 File Offset: 0x00069C06
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || def.IsIngestible;
		}

		// Token: 0x0600A1EF RID: 41455 RVA: 0x002F4B18 File Offset: 0x002F2D18
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible);
		}
	}
}
