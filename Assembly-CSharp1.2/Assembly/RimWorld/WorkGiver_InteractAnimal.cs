using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D41 RID: 3393
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004D92 RID: 19858 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x001AEFCC File Offset: 0x001AD1CC
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x001AF040 File Offset: 0x001AD240
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			if (!pawn.CanReserve(animal, 1, -1, null, forced))
			{
				return false;
			}
			if (animal.Downed)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans, null);
				return false;
			}
			if (!animal.Awake())
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans, null);
				return false;
			}
			if (!animal.CanCasuallyInteractNow(false))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans, null);
				return false;
			}
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			if (num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				JobFailReason.Is("AnimalsSkillTooLow".Translate(num), null);
				return false;
			}
			return true;
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x001AF0E0 File Offset: 0x001AD2E0
		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (tamee.WillEat(thing, pawn, true) && thing.def.ingestible.preferability <= FoodPreferability.RawTasty && !thing.def.IsDrug)
				{
					for (int j = 0; j < thing.stackCount; j++)
					{
						num3 += thing.GetStatValue(StatDefOf.Nutrition, true);
						if (num3 >= num2)
						{
							num++;
							num3 = 0f;
						}
						if (num >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x001AF194 File Offset: 0x001AD394
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp = new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f);
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined);
			FoodUtility.bestFoodSourceOnMap_minNutrition_NewTemp = null;
			if (thing == null)
			{
				return null;
			}
			float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f;
			float nutrition = FoodUtility.GetNutrition(thing, foodDef);
			int count = Mathf.CeilToInt(num / nutrition);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = count;
			return job;
		}

		// Token: 0x040032E5 RID: 13029
		protected static string NoUsableFoodTrans;

		// Token: 0x040032E6 RID: 13030
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x040032E7 RID: 13031
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x040032E8 RID: 13032
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x040032E9 RID: 13033
		private static string CantInteractAnimalBusyTrans;
	}
}
