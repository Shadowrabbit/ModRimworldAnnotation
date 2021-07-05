using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110F RID: 4367
	public class CompBiosculpterPod_AgeReversalCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x060068DF RID: 26847 RVA: 0x00236618 File Offset: 0x00234818
		public override void CycleCompleted(Pawn pawn)
		{
			int num = 3600000;
			long val = (long)(3600000f * pawn.RaceProps.lifeStageAges.Last<LifeStageAge>().minAge);
			pawn.ageTracker.AgeBiologicalTicks = Math.Max(val, pawn.ageTracker.AgeBiologicalTicks - (long)num);
			pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
			int num2 = (int)(pawn.ageTracker.AgeReversalDemandedDeadlineTicks / 60000L);
			Messages.Message("BiosculpterAgeReversalCompletedMessage".Translate(pawn.Named("PAWN"), num2.Named("DEADLINE")), pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
