using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7A RID: 3962
	public class RitualOutcomeEffectWorker_Consumable : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DE6 RID: 24038 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Consumable()
		{
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Consumable(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x00203970 File Offset: 0x00201B70
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(outcome.BestPositiveOutcome(jobRitual) ? 1f : 0.5f))
			{
				Pawn inspiredPawn = (from p in totalPresence.Keys
				where !p.Inspired && DefDatabase<InspirationDef>.AllDefsListForReading.Any((InspirationDef i) => i.Worker.InspirationCanOccur(p))
				select p).RandomElementWithFallback(null);
				if (inspiredPawn == null)
				{
					return;
				}
				InspirationDef inspirationDef = (from i in DefDatabase<InspirationDef>.AllDefsListForReading
				where i.Worker.InspirationCanOccur(inspiredPawn)
				select i).RandomElementWithFallback(null);
				if (inspirationDef == null)
				{
					Log.Error("Could not find inspiration for pawn " + inspiredPawn.Name.ToStringFull);
					return;
				}
				if (!inspiredPawn.mindState.inspirationHandler.TryStartInspiration(inspirationDef, null, false))
				{
					Log.Error("Inspiring " + inspiredPawn.Name.ToStringFull + " failed, but the inspiration worker claimed it can occur!");
					return;
				}
				extraOutcomeDesc = "RitualOutcomeExtraDesc_ConsumableInspiration".Translate(inspiredPawn.Named("PAWN"), inspirationDef.LabelCap.Named("INSPIRATION"), jobRitual.Ritual.Label.Named("RITUAL")).CapitalizeFirst() + " " + inspiredPawn.Inspiration.LetterText;
			}
		}

		// Token: 0x04003633 RID: 13875
		public const float InspirationGainChance = 0.5f;

		// Token: 0x04003634 RID: 13876
		public const float InspirationGainChanceBestOutcome = 1f;
	}
}
