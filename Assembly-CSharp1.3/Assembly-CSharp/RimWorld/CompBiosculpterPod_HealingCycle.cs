using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110E RID: 4366
	public class CompBiosculpterPod_HealingCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x060068DD RID: 26845 RVA: 0x00236560 File Offset: 0x00234760
		public override void CycleCompleted(Pawn pawn)
		{
			if (pawn.health == null)
			{
				return;
			}
			HealthUtility.HealNonPermanentInjuriesAndFreshWounds(pawn);
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_Injury hediff_Injury = HealthUtility.HealRandomPermanentInjury(pawn);
			if (hediff_Injury != null)
			{
				Messages.Message("BiosculpterHealCompletedWithCureMessage".Translate(pawn.Named("PAWN"), hediff_Injury.Named("HEDIFF")), pawn, MessageTypeDefOf.PositiveEvent, true);
				return;
			}
			Messages.Message("BiosculpterHealCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
