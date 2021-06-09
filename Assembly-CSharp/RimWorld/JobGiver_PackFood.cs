using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CDC RID: 3292
	public class JobGiver_PackFood : ThinkNode_JobGiver
	{
		// Token: 0x06004BEC RID: 19436 RVA: 0x001A7EAC File Offset: 0x001A60AC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			float invNutrition = this.GetInventoryPackableFoodNutrition(pawn);
			if (invNutrition > 0.4f)
			{
				return null;
			}
			if (pawn.Map.resourceCounter.TotalHumanEdibleNutrition < (float)pawn.Map.mapPawns.ColonistsSpawnedCount * 1.5f)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 20f, delegate(Thing t)
			{
				if (!this.IsGoodPackableFoodFor(t, pawn) || t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, false) || !t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (invNutrition + t.GetStatValue(StatDefOf.Nutrition, true) * (float)t.stackCount < 0.8f)
				{
					return false;
				}
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(pawn, t, FoodUtility.GetFinalIngestibleDef(t, false));
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].stages[0].baseMoodEffect < 0f)
					{
						return false;
					}
				}
				return true;
			}, (Thing x) => FoodUtility.FoodOptimality(pawn, x, FoodUtility.GetFinalIngestibleDef(x, false), 0f, false), 24, 30);
			if (thing == null)
			{
				return null;
			}
			int num = Mathf.FloorToInt((pawn.needs.food.MaxLevel - invNutrition) / thing.GetStatValue(StatDefOf.Nutrition, true));
			num = Mathf.Min(num, thing.stackCount);
			num = Mathf.Max(num, 1);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = num;
			return job;
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x001A7FE4 File Offset: 0x001A61E4
		private float GetInventoryPackableFoodNutrition(Pawn pawn)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			float num = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.IsGoodPackableFoodFor(innerContainer[i], pawn))
				{
					num += innerContainer[i].GetStatValue(StatDefOf.Nutrition, true) * (float)innerContainer[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x000360C5 File Offset: 0x000342C5
		private bool IsGoodPackableFoodFor(Thing food, Pawn forPawn)
		{
			return food.def.IsNutritionGivingIngestible && food.def.EverHaulable && food.def.ingestible.preferability >= FoodPreferability.MealAwful && forPawn.WillEat(food, null, false);
		}

		// Token: 0x04003214 RID: 12820
		private const float MaxInvNutritionToConsiderLookingForFood = 0.4f;

		// Token: 0x04003215 RID: 12821
		private const float MinFinalInvNutritionToPickUp = 0.8f;

		// Token: 0x04003216 RID: 12822
		private const float MinNutritionPerColonistToDo = 1.5f;

		// Token: 0x04003217 RID: 12823
		public const FoodPreferability MinFoodPreferability = FoodPreferability.MealAwful;
	}
}
