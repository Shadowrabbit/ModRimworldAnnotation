using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BA RID: 698
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityPerDay
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x0006BC1A File Offset: 0x00069E1A
		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)this.props;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0006BC27 File Offset: 0x00069E27
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.TProps.disappearsAtTotalTendQuality >= 0 && this.totalTendQuality >= (float)this.TProps.disappearsAtTotalTendQuality);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0006BC5A File Offset: 0x00069E5A
		public bool IsTended
		{
			get
			{
				return Current.ProgramState == ProgramState.Playing && this.tendTicksLeft > 0;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0006BC6F File Offset: 0x00069E6F
		public bool AllowTend
		{
			get
			{
				if (this.TProps.TendIsPermanent)
				{
					return !this.IsTended;
				}
				return this.TProps.TendTicksOverlap > this.tendTicksLeft;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0006BC9C File Offset: 0x00069E9C
		public override string CompTipStringExtra
		{
			get
			{
				if (this.parent.IsPermanent())
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (!this.IsTended)
				{
					if (!base.Pawn.Dead && this.parent.TendableNow(false))
					{
						stringBuilder.AppendLine("NeedsTendingNow".Translate());
					}
				}
				else
				{
					if (this.TProps.showTendQuality)
					{
						string text;
						if (this.parent.Part != null && this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
						{
							text = this.TProps.labelSolidTendedWell;
						}
						else if (this.parent.Part != null && this.parent.Part.depth == BodyPartDepth.Inside)
						{
							text = this.TProps.labelTendedWellInner;
						}
						else
						{
							text = this.TProps.labelTendedWell;
						}
						if (text != null)
						{
							stringBuilder.AppendLine(text.CapitalizeFirst() + " (" + "Quality".Translate().ToLower() + " " + this.tendQuality.ToStringPercent("F0") + ")");
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}: {1}", "TendQuality".Translate(), this.tendQuality.ToStringPercent()));
						}
					}
					if (!base.Pawn.Dead && !this.TProps.TendIsPermanent && this.parent.TendableNow(true))
					{
						int num = this.tendTicksLeft - this.TProps.TendTicksOverlap;
						if (num < 0)
						{
							stringBuilder.AppendLine("CanTendNow".Translate());
						}
						else if ("NextTendIn".CanTranslate())
						{
							stringBuilder.AppendLine("NextTendIn".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
						}
						else
						{
							stringBuilder.AppendLine("NextTreatmentIn".Translate(num.ToStringTicksToPeriod(true, false, true, true)));
						}
						stringBuilder.AppendLine("TreatmentExpiresIn".Translate(this.tendTicksLeft.ToStringTicksToPeriod(true, false, true, true)));
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0006BF1C File Offset: 0x0006A11C
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.parent is Hediff_Injury)
				{
					if (this.IsTended && !this.parent.IsPermanent())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(this.parent is Hediff_MissingPart) && !this.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					return HediffComp_TendDuration.TendedIcon_Need_General;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0006BFCE File Offset: 0x0006A1CE
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTicksLeft, "tendTicksLeft", -1, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<float>(ref this.totalTendQuality, "totalTendQuality", 0f, false);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0006C00E File Offset: 0x0006A20E
		public override float SeverityChangePerDay()
		{
			if (this.IsTended)
			{
				return this.TProps.severityPerDayTended * this.tendQuality;
			}
			return 0f;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0006C030 File Offset: 0x0006A230
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.tendTicksLeft > 0 && !this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft--;
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006C060 File Offset: 0x0006A260
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp(quality + Rand.Range(-0.25f, 0.25f), 0f, maxQuality);
			this.totalTendQuality += this.tendQuality;
			if (this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft = 1;
			}
			else
			{
				this.tendTicksLeft = Mathf.Max(0, this.tendTicksLeft) + this.TProps.TendTicksFull;
			}
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = "TextMote_Tended".Translate(this.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + this.tendQuality.ToStringPercent();
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(this.parent);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0006C184 File Offset: 0x0006A384
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (!this.TProps.TendIsPermanent)
				{
					stringBuilder.AppendLine("tendTicksLeft: " + this.tendTicksLeft);
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString());
			if (this.TProps.disappearsAtTotalTendQuality >= 0)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"totalTendQuality: ",
					this.totalTendQuality.ToString("F2"),
					" / ",
					this.TProps.disappearsAtTotalTendQuality
				}));
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x04000E37 RID: 3639
		public int tendTicksLeft = -1;

		// Token: 0x04000E38 RID: 3640
		public float tendQuality;

		// Token: 0x04000E39 RID: 3641
		private float totalTendQuality;

		// Token: 0x04000E3A RID: 3642
		public const float TendQualityRandomVariance = 0.25f;

		// Token: 0x04000E3B RID: 3643
		private static readonly Color UntendedColor = new ColorInt(116, 101, 72).ToColor;

		// Token: 0x04000E3C RID: 3644
		private static readonly Texture2D TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);

		// Token: 0x04000E3D RID: 3645
		private static readonly Texture2D TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);

		// Token: 0x04000E3E RID: 3646
		private static readonly Texture2D TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);
	}
}
