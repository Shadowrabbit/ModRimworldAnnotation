using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182D RID: 6189
	public class CompRottable : ThingComp
	{
		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600892A RID: 35114 RVA: 0x0005C1B8 File Offset: 0x0005A3B8
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600892B RID: 35115 RVA: 0x0005C1C5 File Offset: 0x0005A3C5
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600892C RID: 35116 RVA: 0x0005C1DA File Offset: 0x0005A3DA
		// (set) Token: 0x0600892D RID: 35117 RVA: 0x0005C1E2 File Offset: 0x0005A3E2
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

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x0600892E RID: 35118 RVA: 0x0005C1FF File Offset: 0x0005A3FF
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

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600892F RID: 35119 RVA: 0x0028167C File Offset: 0x0027F87C
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06008930 RID: 35120 RVA: 0x002816A4 File Offset: 0x0027F8A4
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

		// Token: 0x06008931 RID: 35121 RVA: 0x0005C22E File Offset: 0x0005A42E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06008932 RID: 35122 RVA: 0x0005C237 File Offset: 0x0005A437
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
		}

		// Token: 0x06008933 RID: 35123 RVA: 0x0005C255 File Offset: 0x0005A455
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x06008934 RID: 35124 RVA: 0x0005C25E File Offset: 0x0005A45E
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x06008935 RID: 35125 RVA: 0x002816D8 File Offset: 0x0027F8D8
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
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					return;
				}
				if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}

		// Token: 0x06008936 RID: 35126 RVA: 0x00281898 File Offset: 0x0027FA98
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x002818DC File Offset: 0x0027FADC
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x06008938 RID: 35128 RVA: 0x0005C26B File Offset: 0x0005A46B
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x06008939 RID: 35129 RVA: 0x0005C283 File Offset: 0x0005A483
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh && FoodUtility.GetFoodPoisonChanceFactor(ingester) > 1E-45f)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
			}
		}

		// Token: 0x0600893A RID: 35130 RVA: 0x00281920 File Offset: 0x0027FB20
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

		// Token: 0x0600893B RID: 35131 RVA: 0x00281A98 File Offset: 0x0027FC98
		public int ApproxTicksUntilRotWhenAtTempOfTile(int tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x0600893C RID: 35132 RVA: 0x00281AB4 File Offset: 0x0027FCB4
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

		// Token: 0x0600893D RID: 35133 RVA: 0x00281B0C File Offset: 0x0027FD0C
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x0600893E RID: 35134 RVA: 0x0005C2A7 File Offset: 0x0005A4A7
		public void RotImmediately()
		{
			if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x04005809 RID: 22537
		private float rotProgressInt;
	}
}
