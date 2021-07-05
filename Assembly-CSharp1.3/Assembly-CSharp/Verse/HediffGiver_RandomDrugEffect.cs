using System;

namespace Verse
{
	// Token: 0x020002DA RID: 730
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x060013A0 RID: 5024 RVA: 0x0006F668 File Offset: 0x0006D868
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if ((this.severityToMtbDaysCurve == null && cause.Severity <= this.minSeverity) || (this.severityToMtbDaysCurve != null && cause.Severity <= this.severityToMtbDaysCurve.Points[0].y))
			{
				return;
			}
			if (Rand.MTBEventOccurs((this.severityToMtbDaysCurve != null) ? this.severityToMtbDaysCurve.Evaluate(cause.Severity) : this.baseMtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x04000E72 RID: 3698
		public SimpleCurve severityToMtbDaysCurve;

		// Token: 0x04000E73 RID: 3699
		public float baseMtbDays;

		// Token: 0x04000E74 RID: 3700
		public float minSeverity;
	}
}
