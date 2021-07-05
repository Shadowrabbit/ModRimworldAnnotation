using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A3 RID: 675
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x0006AB42 File Offset: 0x00068D42
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0006AB4F File Offset: 0x00068D4F
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0006AB5C File Offset: 0x00068D5C
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0006ABB0 File Offset: 0x00068DB0
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0006AC0F File Offset: 0x00068E0F
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0006AC48 File Offset: 0x00068E48
		public override float SeverityChangePerDay()
		{
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
				return this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
			case HediffGrowthMode.Stable:
				return 0f;
			case HediffGrowthMode.Remission:
				return this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
			default:
				throw new NotImplementedException("GrowthMode");
			}
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0006ACA8 File Offset: 0x00068EA8
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				switch (this.growthMode)
				{
				case HediffGrowthMode.Growing:
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					return;
				case HediffGrowthMode.Stable:
					Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					return;
				case HediffGrowthMode.Remission:
					Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(base.Pawn.LabelShort, base.Def.label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0006AE10 File Offset: 0x00069010
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity >= base.Def.maxSeverity) ? " (reached max)" : ""));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x04000E0C RID: 3596
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x04000E0D RID: 3597
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x04000E0E RID: 3598
		public HediffGrowthMode growthMode;

		// Token: 0x04000E0F RID: 3599
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04000E10 RID: 3600
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
