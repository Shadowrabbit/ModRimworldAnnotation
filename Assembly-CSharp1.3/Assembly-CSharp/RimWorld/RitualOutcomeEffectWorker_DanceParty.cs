using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F78 RID: 3960
	public class RitualOutcomeEffectWorker_DanceParty : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DE0 RID: 24032 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_DanceParty()
		{
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_DanceParty(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x002037A0 File Offset: 0x002019A0
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.1f))
			{
				float statFactorFromList = HediffDefOf.WorkFocus.stages[0].statOffsets.GetStatFactorFromList(StatDefOf.WorkSpeedGlobal);
				extraOutcomeDesc = "RitualOutcomeExtraDesc_DancePartyWorkFocus".Translate((((statFactorFromList > 0f) ? "+" : "") + statFactorFromList.ToStringPercent()).Named("PERCENTAGE"));
				foreach (Pawn pawn in totalPresence.Keys)
				{
					pawn.health.AddHediff(HediffDefOf.WorkFocus, null, null, null);
				}
			}
		}

		// Token: 0x04003631 RID: 13873
		public const float WorkFocusChance = 0.1f;
	}
}
