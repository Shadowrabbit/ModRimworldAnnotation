using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FB RID: 1019
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityPerDay
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x0001796E File Offset: 0x00015B6E
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x0001797B File Offset: 0x00015B7B
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.FullyImmune)
				{
					return "DevelopedImmunityLower".Translate();
				}
				return null;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x000E0338 File Offset: 0x000DE538
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + (Mathf.Floor(this.Immunity * 100f) / 100f).ToStringPercent();
				}
				return null;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x00017996 File Offset: 0x00015B96
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def);
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x000179B3 File Offset: 0x00015BB3
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x000179C5 File Offset: 0x00015BC5
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.FullyImmune)
				{
					return HediffComp_Immunizable.IconImmune;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000179DF File Offset: 0x00015BDF
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000179FE File Offset: 0x00015BFE
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00017A1C File Offset: 0x00015C1C
		protected override float SeverityChangePerDay()
		{
			if (!this.FullyImmune)
			{
				return this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor;
			}
			return this.Props.severityPerDayImmune;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x000E0398 File Offset: 0x000DE598
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("  pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true), ToStringNumberSense.Absolute, true));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040012B7 RID: 4791
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x040012B8 RID: 4792
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
