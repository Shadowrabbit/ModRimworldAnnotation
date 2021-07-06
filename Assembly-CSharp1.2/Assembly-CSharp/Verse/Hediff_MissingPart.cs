using System;
using System.Text;

namespace Verse
{
	// Token: 0x020003B5 RID: 949
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x000DCC80 File Offset: 0x000DAE80
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

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x000DCCF8 File Offset: 0x000DAEF8
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

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x000DCDD0 File Offset: 0x000DAFD0
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

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x000DCE28 File Offset: 0x000DB028
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

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x000DCE90 File Offset: 0x000DB090
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x000DCEF0 File Offset: 0x000DB0F0
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x000169D8 File Offset: 0x00014BD8
		// (set) Token: 0x0600179E RID: 6046 RVA: 0x000169ED File Offset: 0x00014BED
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				this.isFreshInt = value;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x000DCF5C File Offset: 0x000DB15C
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x000169F6 File Offset: 0x00014BF6
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00016A08 File Offset: 0x00014C08
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000DCFC0 File Offset: 0x000DB1C0
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

		// Token: 0x060017A3 RID: 6051 RVA: 0x00016A10 File Offset: 0x00014C10
		public override void Tended_NewTemp(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended_NewTemp(quality, maxQuality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000DCFF4 File Offset: 0x000DB1F4
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

		// Token: 0x060017A5 RID: 6053 RVA: 0x000DD0BC File Offset: 0x000DB2BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04001216 RID: 4630
		public HediffDef lastInjury;

		// Token: 0x04001217 RID: 4631
		private bool isFreshInt;
	}
}
