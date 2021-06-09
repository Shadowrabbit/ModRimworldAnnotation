using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000408 RID: 1032
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x0600191F RID: 6431 RVA: 0x000E1344 File Offset: 0x000DF544
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

		// Token: 0x06001920 RID: 6432 RVA: 0x00017C2C File Offset: 0x00015E2C
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

		// Token: 0x040012D3 RID: 4819
		public float severityAmount = float.NaN;

		// Token: 0x040012D4 RID: 4820
		public float mtbHours = -1f;

		// Token: 0x040012D5 RID: 4821
		private static int mtbCheckInterval = 1200;
	}
}
