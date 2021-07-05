using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D2 RID: 722
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x06001388 RID: 5000 RVA: 0x0006EDF4 File Offset: 0x0006CFF4
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.IsNestedHashIntervalTick(60, HediffGiver_AddSeverity.mtbCheckInterval) && Rand.MTBEventOccurs(this.mtbHours, 2500f, (float)HediffGiver_AddSeverity.mtbCheckInterval))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false).Severity += this.severityAmount;
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0006EE63 File Offset: 0x0006D063
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (float.IsNaN(this.severityAmount))
			{
				yield return "severityAmount is not defined";
			}
			if (this.mtbHours < 0f)
			{
				yield return "mtbHours is not defined";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000E63 RID: 3683
		public float severityAmount = float.NaN;

		// Token: 0x04000E64 RID: 3684
		public float mtbHours = -1f;

		// Token: 0x04000E65 RID: 3685
		private static int mtbCheckInterval = 1200;
	}
}
