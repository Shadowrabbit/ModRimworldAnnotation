using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118A RID: 4490
	public class CompRottable : ThingComp
	{
		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06006BF8 RID: 27640 RVA: 0x002435E3 File Offset: 0x002417E3
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06006BF9 RID: 27641 RVA: 0x002435F0 File Offset: 0x002417F0
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06006BFA RID: 27642 RVA: 0x00243605 File Offset: 0x00241805
		// (set) Token: 0x06006BFB RID: 27643 RVA: 0x0024360D File Offset: 0x0024180D
		public float RotProgress
		{
			get
			{
				return this.rotProgressInt;
			}
			set
			{
				RotStage stage = this.Stage;
				this.rotProgressInt = value;
				if (stage != this.Stage)
				{
					this.StageChanged();
				}
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06006BFC RID: 27644 RVA: 0x0024362A File Offset: 0x0024182A
		public RotStage Stage
		{
			get
			{
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					return RotStage.Fresh;
				}
				if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					return RotStage.Rotting;
				}
				return RotStage.Dessicated;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06006BFD RID: 27645 RVA: 0x0024365C File Offset: 0x0024185C
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06006BFE RID: 27646 RVA: 0x00243684 File Offset: 0x00241884
		public bool Active
		{
			get
			{
				if (this.PropsRot.disableIfHatcher)
				{
					CompHatcher compHatcher = this.parent.TryGetComp<CompHatcher>();
					if (compHatcher != null && !compHatcher.TemperatureDamaged)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06006BFF RID: 27647 RVA: 0x002436B8 File Offset: 0x002418B8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x002436C1 File Offset: 0x002418C1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x002436DF File Offset: 0x002418DF
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x002436E8 File Offset: 0x002418E8
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x002436F8 File Offset: 0x002418F8
		private void Tick(int interval)
		{
			if (!this.Active)
			{
				return;
			}
			float rotProgress = this.RotProgress;
			float num = GenTemperature.RotRateAtTemperature(this.parent.AmbientTemperature);
			this.RotProgress += num * (float)interval;
			if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
			{
				if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
				{
					Messages.Message("MessageRottedAwayInStorage".Translate(this.parent.Label, this.parent).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
				}
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f) && this.ShouldTakeRotDamage())
			{
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
					return;
				}
				if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}
			}
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x002438BC File Offset: 0x00241ABC
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x00243900 File Offset: 0x00241B00
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x00243943 File Offset: 0x00241B43
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x0024395B File Offset: 0x00241B5B
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh && FoodUtility.GetFoodPoisonChanceFactor(ingester) > 1E-45f)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
			}
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x00243980 File Offset: 0x00241B80
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.Stage)
			{
			case RotStage.Fresh:
				stringBuilder.Append("RotStateFresh".Translate() + ".");
				break;
			case RotStage.Rotting:
				stringBuilder.Append("RotStateRotting".Translate() + ".");
				break;
			case RotStage.Dessicated:
				stringBuilder.Append("RotStateDessicated".Translate() + ".");
				break;
			}
			if ((float)this.PropsRot.TicksToRotStart - this.RotProgress > 0f)
			{
				float num = GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(this.parent.AmbientTemperature));
				int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
				stringBuilder.AppendLine();
				if (num < 0.001f)
				{
					stringBuilder.Append("CurrentlyFrozen".Translate() + ".");
				}
				else if (num < 0.999f)
				{
					stringBuilder.Append("CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true)) + ".");
				}
				else
				{
					stringBuilder.Append("NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true)) + ".");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x00243AF8 File Offset: 0x00241CF8
		public int ApproxTicksUntilRotWhenAtTempOfTile(int tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x00243B14 File Offset: 0x00241D14
		public int TicksUntilRotAtTemp(float temp)
		{
			if (!this.Active)
			{
				return 72000000;
			}
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0f)
			{
				return 72000000;
			}
			float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num2 <= 0f)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x00243B6C File Offset: 0x00241D6C
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x00243B8E File Offset: 0x00241D8E
		public void RotImmediately()
		{
			if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x04003C14 RID: 15380
		private float rotProgressInt;
	}
}
