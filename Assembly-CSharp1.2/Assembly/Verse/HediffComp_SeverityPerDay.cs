using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FD RID: 1021
	public class HediffComp_SeverityPerDay : HediffComp
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00017A81 File Offset: 0x00015C81
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x000E0480 File Offset: 0x000DE680
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

		// Token: 0x060018DA RID: 6362 RVA: 0x00017A8E File Offset: 0x00015C8E
		protected virtual float SeverityChangePerDay()
		{
			return this.Props.severityPerDay;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x000E04BC File Offset: 0x000DE6BC
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

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x000E0530 File Offset: 0x000DE730
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

		// Token: 0x060018DD RID: 6365 RVA: 0x000E05A0 File Offset: 0x000DE7A0
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

		// Token: 0x040012BC RID: 4796
		protected const int SeverityUpdateInterval = 200;
	}
}
