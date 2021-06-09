using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B4 RID: 948
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x000DC784 File Offset: 0x000DA984
		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x000DC7B0 File Offset: 0x000DA9B0
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x000DC824 File Offset: 0x000DAA24
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.source.label);
					if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(this.sourceBodyPartGroup.LabelShort);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x00016925 File Offset: 0x00014B25
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x000DC92C File Offset: 0x000DAB2C
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x0001693A File Offset: 0x00014B3A
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x000DC95C File Offset: 0x000DAB5C
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					return this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				return this.Severity * this.def.injuryProps.painPerSeverity;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000DC9EC File Offset: 0x000DABEC
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					return 0f;
				}
				float num = this.Severity * this.def.injuryProps.bleedRate;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000DCAB4 File Offset: 0x000DACB4
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x0001696A File Offset: 0x00014B6A
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000DCB04 File Offset: 0x000DAD04
		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000DCB38 File Offset: 0x000DAD38
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000DCB94 File Offset: 0x000DAD94
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000DCC04 File Offset: 0x000DAE04
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f)
			{
				Log.Error(string.Concat(new object[]
				{
					"Added injury to ",
					base.Part.def,
					" but it should be impossible to hit it. pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					" dinfo=",
					dinfo.ToStringSafe<DamageInfo?>()
				}), false);
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0001697D File Offset: 0x00014B7D
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04001215 RID: 4629
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
