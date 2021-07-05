using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F79 RID: 3961
	public class RitualOutcomeEffectWorker_Blinding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DE3 RID: 24035 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Blinding()
		{
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Blinding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x00203880 File Offset: 0x00201A80
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (ModsConfig.RoyaltyActive && outcome.Positive && (outcome.BestPositiveOutcome(jobRitual) || Rand.Chance(0.5f)))
			{
				Pawn pawn = ((LordJob_Ritual_Mutilation)jobRitual).mutilatedPawns[0];
				extraOutcomeDesc = "RitualOutcomeExtraDesc_BlindingPsylink".Translate(pawn.Named("PAWN"));
				List<Ability> existingAbils = pawn.abilities.AllAbilitiesForReading.ToList<Ability>();
				pawn.ChangePsylinkLevel(1, true);
				Ability ability = pawn.abilities.AllAbilitiesForReading.FirstOrDefault((Ability a) => !existingAbils.Contains(a));
				if (ability != null)
				{
					extraOutcomeDesc += " " + "RitualOutcomeExtraDesc_BlindingPsylinkAbility".Translate(ability.def.LabelCap, pawn.Named("PAWN"));
				}
			}
		}

		// Token: 0x04003632 RID: 13874
		public const float PsylinkGainChance = 0.5f;
	}
}
