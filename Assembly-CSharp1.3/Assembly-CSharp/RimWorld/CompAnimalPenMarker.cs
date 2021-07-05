using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F9 RID: 4345
	public class CompAnimalPenMarker : ThingComp
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06006822 RID: 26658 RVA: 0x0023385D File Offset: 0x00231A5D
		public CompProperties_AnimalPenMarker Props
		{
			get
			{
				return this.props as CompProperties_AnimalPenMarker;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006823 RID: 26659 RVA: 0x0023386A File Offset: 0x00231A6A
		public PenMarkerState PenState
		{
			get
			{
				ThingWithComps parent = this.parent;
				if (parent == null)
				{
					return null;
				}
				Map map = parent.Map;
				if (map == null)
				{
					return null;
				}
				return map.animalPenManager.GetPenMarkerState(this);
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06006824 RID: 26660 RVA: 0x0023388E File Offset: 0x00231A8E
		public ThingFilter AnimalFilter
		{
			get
			{
				return this.animalFilter;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06006825 RID: 26661 RVA: 0x00233896 File Offset: 0x00231A96
		public ThingFilter AutoCutFilter
		{
			get
			{
				return this.autoCutFilter;
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06006826 RID: 26662 RVA: 0x0023389E File Offset: 0x00231A9E
		public List<ThingDef> ForceDisplayedAnimalDefs
		{
			get
			{
				return this.forceDisplayedAnimalDefs;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06006827 RID: 26663 RVA: 0x002338A8 File Offset: 0x00231AA8
		public PenFoodCalculator PenFoodCalculator
		{
			get
			{
				if (this.cachedFoodCalculator == null)
				{
					this.cachedFoodCalculator = new PenFoodCalculator();
					this.cachedFoodCalculator_cachedAtTick = null;
				}
				if (this.cachedFoodCalculator_cachedAtTick != null)
				{
					int ticksGame = Find.TickManager.TicksGame;
					int? num = this.cachedFoodCalculator_cachedAtTick + 20;
					if (!(ticksGame > num.GetValueOrDefault() & num != null))
					{
						goto IL_95;
					}
				}
				this.cachedFoodCalculator.ResetAndProcessPen(this);
				this.cachedFoodCalculator_cachedAtTick = new int?(Find.TickManager.TicksGame);
				IL_95:
				return this.cachedFoodCalculator;
			}
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x00233950 File Offset: 0x00231B50
		public void AddForceDisplayedAnimal(ThingDef animalDef)
		{
			if (!this.forceDisplayedAnimalDefs.Contains(animalDef))
			{
				this.forceDisplayedAnimalDefs.Add(animalDef);
			}
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x0023396C File Offset: 0x00231B6C
		public void RemoveForceDisplayedAnimal(ThingDef animalDef)
		{
			this.forceDisplayedAnimalDefs.Remove(animalDef);
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x0023397B File Offset: 0x00231B7B
		public bool AcceptsToPen(Pawn animal)
		{
			return this.animalFilter.Allows(animal) && AnimalPenUtility.GetFixedAnimalFilter().Allows(animal);
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x00233998 File Offset: 0x00231B98
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.autoCutFilter == null)
			{
				this.autoCutFilter = new ThingFilter();
				this.autoCutFilter.CopyAllowancesFrom(this.parent.Map.animalPenManager.GetDefaultAutoCutFilter());
			}
			if (!respawningAfterLoad && this.label.NullOrEmpty())
			{
				this.label = (this.parent.Map.animalPenManager.MakeNewAnimalPenName() ?? "AnimalPenMarkerDefaultLabel".Translate(1));
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Cow);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Goat);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Chicken);
				this.autoCutFilter.CopyAllowancesFrom(this.parent.Map.animalPenManager.GetDefaultAutoCutFilter());
				this.animalFilter.CopyAllowancesFrom(AnimalPenUtility.GetDefaultAnimalFilter());
			}
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x00233A89 File Offset: 0x00231C89
		public override void CompTickLong()
		{
			base.CompTickLong();
			if (!this.autoCut)
			{
				return;
			}
			if (CompAnimalPenMarker.CheckIfMidnight(this.parent, this.lastCheckedAutoCutTick))
			{
				this.lastCheckedAutoCutTick = new int?(Find.TickManager.TicksGame);
				this.DesignatePlantsToCut();
			}
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x00233AC8 File Offset: 0x00231CC8
		private static bool CheckIfMidnight(Thing thing, int? lastCheckTicks)
		{
			if (GenLocalDate.HourInteger(thing) != 0)
			{
				return false;
			}
			if (lastCheckTicks != null)
			{
				int? num = lastCheckTicks + 30000;
				int ticksGame = Find.TickManager.TicksGame;
				return num.GetValueOrDefault() < ticksGame & num != null;
			}
			return true;
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x00233B30 File Offset: 0x00231D30
		public void DesignatePlantsToCut()
		{
			if (this.PenState.Unenclosed)
			{
				return;
			}
			Map map = this.parent.Map;
			foreach (Region region in this.PenState.DirectlyConnectedRegions)
			{
				foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.Plant))
				{
					Plant plant;
					if (thing.GetRegion(RegionType.Set_Passable) == region && thing.def.plant.allowAutoCut && ((plant = (thing as Plant)) == null || !plant.DeliberatelyCultivated()) && this.autoCutFilter.Allows(thing) && map.animalPenManager.GetFixedAutoCutFilter().Allows(thing) && !map.designationManager.HasMapDesignationOn(thing))
					{
						map.designationManager.AddDesignation(new Designation(thing, DesignationDefOf.CutPlant));
					}
				}
			}
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x00233C6C File Offset: 0x00231E6C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<bool>(ref this.autoCut, "autoCut", false, false);
			Scribe_Values.Look<int?>(ref this.lastCheckedAutoCutTick, "lastCheckedAutoCutTick", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.animalFilter, "animalFilter", Array.Empty<object>());
			Scribe_Deep.Look<ThingFilter>(ref this.autoCutFilter, "autoCutFilter", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.forceDisplayedAnimalDefs, "forceDisplayedAnimalDefs", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.forceDisplayedAnimalDefs == null)
			{
				this.forceDisplayedAnimalDefs = new List<ThingDef>();
			}
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x00233D18 File Offset: 0x00231F18
		public override string CompInspectStringExtra()
		{
			PenFoodCalculator penFoodCalculator = this.PenFoodCalculator;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.label).Append(": ").AppendLine(penFoodCalculator.PenSizeDescription());
			PenMarkerState penState = this.PenState;
			if (penState.Unenclosed)
			{
				stringBuilder.Append("PenUnenclosedLabel".Translate());
				if (penState.PassableDoors)
				{
					stringBuilder.Append(" (" + "PenOpenDoorLabel".Translate() + ")");
				}
				else
				{
					stringBuilder.Append(".");
				}
				stringBuilder.AppendLine();
			}
			bool flag = penFoodCalculator.SumNutritionConsumptionPerDay > penFoodCalculator.NutritionPerDayToday;
			string value = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.NutritionPerDayToday, false);
			string text = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.SumNutritionConsumptionPerDay, false);
			if (flag)
			{
				text = text.Colorize(Color.red);
			}
			stringBuilder.Append("PenFoodTab_NaturalNutritionGrowthRate".Translate()).Append(": ").AppendLine(value);
			stringBuilder.Append("PenFoodTab_TotalNutritionConsumptionRate".Translate()).Append(": ").AppendLine(text);
			if (penFoodCalculator.sumStockpiledNutritionAvailableNow > 0f)
			{
				stringBuilder.Append("PenFoodTab_StockpileTotal".Translate()).Append(": ").AppendLine(PenFoodCalculator.NutritionToString(penFoodCalculator.sumStockpiledNutritionAvailableNow, false));
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x04003A84 RID: 14980
		private const int FoodCalculatorCacheTicks = 20;

		// Token: 0x04003A85 RID: 14981
		public string label = "";

		// Token: 0x04003A86 RID: 14982
		public bool autoCut;

		// Token: 0x04003A87 RID: 14983
		private int? lastCheckedAutoCutTick;

		// Token: 0x04003A88 RID: 14984
		private ThingFilter animalFilter = new ThingFilter();

		// Token: 0x04003A89 RID: 14985
		private ThingFilter autoCutFilter = new ThingFilter();

		// Token: 0x04003A8A RID: 14986
		private List<ThingDef> forceDisplayedAnimalDefs = new List<ThingDef>();

		// Token: 0x04003A8B RID: 14987
		private PenFoodCalculator cachedFoodCalculator;

		// Token: 0x04003A8C RID: 14988
		private int? cachedFoodCalculator_cachedAtTick;
	}
}
