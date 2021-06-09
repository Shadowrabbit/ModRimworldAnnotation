using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B6 RID: 6070
	public class CompEggLayer : ThingComp
	{
		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x0600862D RID: 34349 RVA: 0x00277E18 File Offset: 0x00276018
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable);
			}
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x0600862E RID: 34350 RVA: 0x0005A06D File Offset: 0x0005826D
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x0600862F RID: 34351 RVA: 0x0005A089 File Offset: 0x00058289
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x06008630 RID: 34352 RVA: 0x0005A0A1 File Offset: 0x000582A1
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.Props.eggProgressUnfertilizedMax < 1f && this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x06008631 RID: 34353 RVA: 0x0005A0D5 File Offset: 0x000582D5
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x06008632 RID: 34354 RVA: 0x00277E64 File Offset: 0x00276064
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x06008633 RID: 34355 RVA: 0x00277EB0 File Offset: 0x002760B0
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

		// Token: 0x06008634 RID: 34356 RVA: 0x0005A0E2 File Offset: 0x000582E2
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x06008635 RID: 34357 RVA: 0x00277F34 File Offset: 0x00276134
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				Log.Error("LayEgg while not Active: " + this.parent, false);
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

		// Token: 0x06008636 RID: 34358 RVA: 0x00278010 File Offset: 0x00276210
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

		// Token: 0x04005675 RID: 22133
		private float eggProgress;

		// Token: 0x04005676 RID: 22134
		private int fertilizationCount;

		// Token: 0x04005677 RID: 22135
		private Pawn fertilizedBy;
	}
}
