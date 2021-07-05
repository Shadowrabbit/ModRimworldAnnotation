using System;

namespace Verse
{
	// Token: 0x020002DC RID: 732
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x0006F77C File Offset: 0x0006D97C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			if (firstHediffOfDef != null)
			{
				firstHediffOfDef.ageTicks = 0;
				return;
			}
			if (base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}
