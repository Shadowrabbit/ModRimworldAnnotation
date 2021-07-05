using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F03 RID: 3843
	public class RitualAttachableOutcomeEffectWorker_GauranlenPosSprout : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BC1 RID: 23489 RVA: 0x001FB7C8 File Offset: 0x001F99C8
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (this.CanApplyNow(jobRitual.Ritual, jobRitual.Map))
			{
				IncidentParms parms = new IncidentParms
				{
					target = jobRitual.Map,
					customLetterText = IncidentDefOf.GauranlenPodSpawn.letterText + "\n\n" + "RitualAttachedOutcome_GauranlenTreePod_ExtraDesc".Translate(jobRitual.RitualLabel)
				};
				if (IncidentDefOf.GauranlenPodSpawn.Worker.TryExecute(parms))
				{
					extraOutcomeDesc = this.def.letterInfoText;
				}
			}
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x001FB860 File Offset: 0x001F9A60
		public override AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			if (!IncidentWorker_GauranlenPodSpawn.IsGoodBiome(map.Biome))
			{
				return "RitualAttachedOutcomeCantApply_ExtremeBiome".Translate();
			}
			IntVec3 intVec;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out intVec))
			{
				return "RitualAttachedOutcomeCantApply_NoValidSpot".Translate();
			}
			return base.CanApplyNow(ritual, map);
		}
	}
}
