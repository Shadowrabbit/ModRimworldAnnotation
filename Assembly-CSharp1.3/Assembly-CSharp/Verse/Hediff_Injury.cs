using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CA RID: 714
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x0006D55C File Offset: 0x0006B75C
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

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x0006D588 File Offset: 0x0006B788
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

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0006D5FC File Offset: 0x0006B7FC
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

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x0006D703 File Offset: 0x0006B903
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x0006D718 File Offset: 0x0006B918
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

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x0006D747 File Offset: 0x0006B947
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

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x0006D778 File Offset: 0x0006B978
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				float num;
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					num = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				else
				{
					num = this.Severity * this.def.injuryProps.painPerSeverity;
				}
				return num / this.pawn.HealthScale;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x0006D818 File Offset: 0x0006BA18
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

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x0006D8E0 File Offset: 0x0006BAE0
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x0006D92D File Offset: 0x0006BB2D
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0006D940 File Offset: 0x0006BB40
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

		// Token: 0x0600134F RID: 4943 RVA: 0x0006D974 File Offset: 0x0006BB74
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

		// Token: 0x06001350 RID: 4944 RVA: 0x0006D9D0 File Offset: 0x0006BBD0
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0006DA40 File Offset: 0x0006BC40
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
				}));
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0006DABB File Offset: 0x0006BCBB
		public override void PostRemoved()
		{
			base.PostRemoved();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0006DADA File Offset: 0x0006BCDA
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04000E52 RID: 3666
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
