using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F01 RID: 3841
	public class RitualAttachableOutcomeEffectWorker_FarmAnimalsWanderIn : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BBD RID: 23485 RVA: 0x001FB6D4 File Offset: 0x001F98D4
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				totalBodySize = (outcome.BestPositiveOutcome(jobRitual) ? 3f : 2f),
				customLetterText = "RitualAttachedOutcome_FarmAnimalsWanderIn_Desc".Translate(jobRitual.RitualLabel)
			};
			if (IncidentDefOf.FarmAnimalsWanderIn.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x04003578 RID: 13688
		public const float PositiveOutcomeBodysize = 2f;

		// Token: 0x04003579 RID: 13689
		public const float BestOutcomeBodysize = 3f;
	}
}
