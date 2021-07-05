using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C0 RID: 704
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x0006C719 File Offset: 0x0006A919
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0006C728 File Offset: 0x0006A928
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(200))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.0033333334f;
				severityAdjustment += num;
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006C763 File Offset: 0x0006A963
		public virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x0006C770 File Offset: 0x0006A970
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.props is HediffCompProperties_SeverityPerDay && this.Props.showHoursToRecover && this.SeverityChangePerDay() < 0f)
				{
					return Mathf.RoundToInt(this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay()) * 24f) + "LetterHour".Translate();
				}
				return null;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x0006C7E4 File Offset: 0x0006A9E4
		public override string CompTipStringExtra
		{
			get
			{
				if (this.props is HediffCompProperties_SeverityPerDay && this.Props.showDaysToRecover && this.SeverityChangePerDay() < 0f)
				{
					return "DaysToRecover".Translate((this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay())).ToString("0.0"));
				}
				return null;
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006C854 File Offset: 0x0006AA54
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000E4C RID: 3660
		protected const int SeverityUpdateInterval = 200;
	}
}
