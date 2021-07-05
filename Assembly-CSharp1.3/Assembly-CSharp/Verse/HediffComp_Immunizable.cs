using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BE RID: 702
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityPerDay
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0006C4A0 File Offset: 0x0006A6A0
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0006C4AD File Offset: 0x0006A6AD
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

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0006C4C8 File Offset: 0x0006A6C8
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + (Mathf.Floor(this.NaturalImmunity * 100f) / 100f).ToStringPercent();
				}
				return null;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x0006C526 File Offset: 0x0006A726
		public float NaturalImmunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, true);
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0006C544 File Offset: 0x0006A744
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, false);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0006C562 File Offset: 0x0006A762
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0006C574 File Offset: 0x0006A774
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

		// Token: 0x06001300 RID: 4864 RVA: 0x0006C58E File Offset: 0x0006A78E
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0006C5AD File Offset: 0x0006A7AD
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0006C5CB File Offset: 0x0006A7CB
		public override float SeverityChangePerDay()
		{
			if (!this.FullyImmune)
			{
				return this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor;
			}
			return this.Props.severityPerDayImmune;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0006C5F4 File Offset: 0x0006A7F4
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

		// Token: 0x04000E47 RID: 3655
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x04000E48 RID: 3656
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
