using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016C3 RID: 5827
	public class Building_NutrientPasteDispenser : Building
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06007FDE RID: 32734 RVA: 0x00055E4C File Offset: 0x0005404C
		public bool CanDispenseNow
		{
			get
			{
				return this.powerComp.PowerOn && this.HasEnoughFeedstockInHoppers();
			}
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06007FDF RID: 32735 RVA: 0x00055E63 File Offset: 0x00054063
		public List<IntVec3> AdjCellsCardinalInBounds
		{
			get
			{
				if (this.cachedAdjCellsCardinal == null)
				{
					this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
					where c.InBounds(base.Map)
					select c).ToList<IntVec3>();
				}
				return this.cachedAdjCellsCardinal;
			}
		}

		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06007FE0 RID: 32736 RVA: 0x00055E95 File Offset: 0x00054095
		public virtual ThingDef DispensableDef
		{
			get
			{
				return ThingDefOf.MealNutrientPaste;
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06007FE1 RID: 32737 RVA: 0x00055E9C File Offset: 0x0005409C
		public override Color DrawColor
		{
			get
			{
				if (!this.IsSociallyProper(null, false, false))
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x06007FE2 RID: 32738 RVA: 0x00055EB5 File Offset: 0x000540B5
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x06007FE3 RID: 32739 RVA: 0x0025E760 File Offset: 0x0025C960
		public virtual Building AdjacentReachableHopper(Pawn reacher)
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Building edifice = this.AdjCellsCardinalInBounds[i].GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.Hopper && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return edifice;
				}
			}
			return null;
		}

		// Token: 0x06007FE4 RID: 32740 RVA: 0x0025E7C0 File Offset: 0x0025C9C0
		public virtual Thing TryDispenseFood()
		{
			if (!this.CanDispenseNow)
			{
				return null;
			}
			float num = this.def.building.nutritionCostPerDispense - 0.0001f;
			List<ThingDef> list = new List<ThingDef>();
			for (;;)
			{
				Thing thing = this.FindFeedInAnyHopper();
				if (thing == null)
				{
					break;
				}
				int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true)));
				num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition, true);
				list.Add(thing.def);
				thing.SplitOff(num2);
				if (num <= 0f)
				{
					goto Block_3;
				}
			}
			Log.Error("Did not find enough food in hoppers while trying to dispense.", false);
			return null;
			Block_3:
			this.def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste, null);
			CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
			for (int i = 0; i < list.Count; i++)
			{
				compIngredients.RegisterIngredient(list[i]);
			}
			return thing2;
		}

		// Token: 0x06007FE5 RID: 32741 RVA: 0x0025E8C0 File Offset: 0x0025CAC0
		public virtual Thing FindFeedInAnyHopper()
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06007FE6 RID: 32742 RVA: 0x0025E94C File Offset: 0x0025CB4C
		public virtual bool HasEnoughFeedstockInHoppers()
		{
			float num = 0f;
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true);
				}
				if (num >= this.def.building.nutritionCostPerDispense)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007FE7 RID: 32743 RVA: 0x0025EA10 File Offset: 0x0025CC10
		public static bool IsAcceptableFeedstock(ThingDef def)
		{
			return def.IsNutritionGivingIngestible && def.ingestible.preferability != FoodPreferability.Undefined && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant && (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}

		// Token: 0x06007FE8 RID: 32744 RVA: 0x0025EA64 File Offset: 0x0025CC64
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetInspectString());
			if (!this.IsSociallyProper(null, false, false))
			{
				stringBuilder.AppendLine("InPrisonCell".Translate());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x040052EA RID: 21226
		public CompPowerTrader powerComp;

		// Token: 0x040052EB RID: 21227
		private List<IntVec3> cachedAdjCellsCardinal;

		// Token: 0x040052EC RID: 21228
		public static int CollectDuration = 50;
	}
}
