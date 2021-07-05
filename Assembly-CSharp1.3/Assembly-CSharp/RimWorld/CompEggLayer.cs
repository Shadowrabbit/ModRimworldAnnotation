using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001127 RID: 4391
	public class CompEggLayer : ThingComp
	{
		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06006977 RID: 26999 RVA: 0x00238C34 File Offset: 0x00236E34
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06006978 RID: 27000 RVA: 0x00238C80 File Offset: 0x00236E80
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06006979 RID: 27001 RVA: 0x00238C9C File Offset: 0x00236E9C
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x0600697A RID: 27002 RVA: 0x00238CB4 File Offset: 0x00236EB4
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.Props.eggProgressUnfertilizedMax < 1f && this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x0600697B RID: 27003 RVA: 0x00238CE8 File Offset: 0x00236EE8
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x00238CF8 File Offset: 0x00236EF8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x00238D44 File Offset: 0x00236F44
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (this.Props.eggLayIntervalDays * 60000f);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.eggProgress += num;
				if (this.eggProgress > 1f)
				{
					this.eggProgress = 1f;
				}
				if (this.ProgressStoppedBecauseUnfertilized)
				{
					this.eggProgress = this.Props.eggProgressUnfertilizedMax;
				}
			}
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x00238DC8 File Offset: 0x00236FC8
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x00238DE2 File Offset: 0x00236FE2
		public ThingDef NextEggType()
		{
			if (this.fertilizationCount > 0)
			{
				return this.Props.eggFertilizedDef;
			}
			return this.Props.eggUnfertilizedDef;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x00238E04 File Offset: 0x00237004
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				Log.Error("LayEgg while not Active: " + this.parent);
			}
			this.eggProgress = 0f;
			int randomInRange = this.Props.eggCountRange.RandomInRange;
			if (randomInRange == 0)
			{
				return null;
			}
			Thing thing;
			if (this.fertilizationCount > 0)
			{
				thing = ThingMaker.MakeThing(this.Props.eggFertilizedDef, null);
				this.fertilizationCount = Mathf.Max(0, this.fertilizationCount - randomInRange);
			}
			else
			{
				thing = ThingMaker.MakeThing(this.Props.eggUnfertilizedDef, null);
			}
			thing.stackCount = randomInRange;
			CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
			if (compHatcher != null)
			{
				compHatcher.hatcheeFaction = this.parent.Faction;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					compHatcher.hatcheeParent = pawn;
				}
				if (this.fertilizedBy != null)
				{
					compHatcher.otherParent = this.fertilizedBy;
				}
			}
			return thing;
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x00238EE0 File Offset: 0x002370E0
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			string text = "EggProgress".Translate() + ": " + this.eggProgress.ToStringPercent();
			if (this.fertilizationCount > 0)
			{
				text += "\n" + "Fertilized".Translate();
			}
			else if (this.ProgressStoppedBecauseUnfertilized)
			{
				text += "\n" + "ProgressStoppedUntilFertilized".Translate();
			}
			return text;
		}

		// Token: 0x04003AF7 RID: 15095
		private float eggProgress;

		// Token: 0x04003AF8 RID: 15096
		private int fertilizationCount;

		// Token: 0x04003AF9 RID: 15097
		private Pawn fertilizedBy;
	}
}
