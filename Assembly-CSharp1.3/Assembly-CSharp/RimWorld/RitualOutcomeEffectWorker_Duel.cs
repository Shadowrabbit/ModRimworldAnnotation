using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7C RID: 3964
	public class RitualOutcomeEffectWorker_Duel : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DED RID: 24045 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Duel()
		{
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Duel(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x00203CA2 File Offset: 0x00201EA2
		protected override bool OutcomePossible(OutcomeChance chance, LordJob_Ritual ritual)
		{
			if (!chance.BestPositiveOutcome(ritual))
			{
				return true;
			}
			return ((LordJob_Ritual_Duel)ritual).duelists.Any((Pawn d) => d.Dead);
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x00203CE0 File Offset: 0x00201EE0
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive)
			{
				float amount = outcome.BestPositiveOutcome(jobRitual) ? 0.5f : 0.25f;
				float xp = outcome.BestPositiveOutcome(jobRitual) ? 5000f : 2500f;
				float xp2 = outcome.BestPositiveOutcome(jobRitual) ? 2000f : 1000f;
				LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)jobRitual;
				foreach (Pawn pawn in totalPresence.Keys)
				{
					if (lordJob_Ritual_Duel.duelists.Contains(pawn))
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp, false);
					}
					else
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp2, false);
						if (pawn.needs.joy != null)
						{
							pawn.needs.joy.GainJoy(amount, JoyKindDefOf.Social);
						}
					}
				}
			}
		}

		// Token: 0x04003638 RID: 13880
		public const float RecreationGainGood = 0.25f;

		// Token: 0x04003639 RID: 13881
		public const float RecreationGainBest = 0.5f;

		// Token: 0x0400363A RID: 13882
		public const float MeleeXPGainParticipantsGood = 2500f;

		// Token: 0x0400363B RID: 13883
		public const float MeleeXPGainSpectatorsGood = 1000f;

		// Token: 0x0400363C RID: 13884
		public const float MeleeXPGainParticipantsBest = 5000f;

		// Token: 0x0400363D RID: 13885
		public const float MeleeXPGainSpectatorsBest = 2000f;
	}
}
