using System;
using System.Text;

namespace Verse
{
	// Token: 0x020002CC RID: 716
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x0006DC98 File Offset: 0x0006BE98
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x0006DD10 File Offset: 0x0006BF10
		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					return this.lastInjury.injuryProps.destroyedOutLabel;
				}
				return this.lastInjury.injuryProps.destroyedLabel;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x0006DDE8 File Offset: 0x0006BFE8
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x0006DE40 File Offset: 0x0006C040
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0006DEA8 File Offset: 0x0006C0A8
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity / this.pawn.HealthScale;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x0006DF14 File Offset: 0x0006C114
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x0006DF80 File Offset: 0x0006C180
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x0006DF95 File Offset: 0x0006C195
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				if (this.isFreshInt == value)
				{
					return;
				}
				this.isFreshInt = value;
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0006DFB4 File Offset: 0x0006C1B4
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x0006E018 File Offset: 0x0006C218
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0006E02A File Offset: 0x0006C22A
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0006E034 File Offset: 0x0006C234
		public override void Tick()
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.Tick();
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0006E068 File Offset: 0x0006C268
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0006E08C File Offset: 0x0006C28C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0006E154 File Offset: 0x0006C354
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04000E54 RID: 3668
		public HediffDef lastInjury;

		// Token: 0x04000E55 RID: 3669
		private bool isFreshInt;
	}
}
