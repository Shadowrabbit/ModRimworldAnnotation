using System;

namespace Verse
{
	// Token: 0x02000415 RID: 1045
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x000E1D10 File Offset: 0x000DFF10
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
