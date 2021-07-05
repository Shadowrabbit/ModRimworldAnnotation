using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000806 RID: 2054
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060036D6 RID: 14038 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x00136D34 File Offset: 0x00134F34
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x00136DA8 File Offset: 0x00134FA8
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			string text;
			if (WorkGiver_InteractAnimal.CanInteractWithAnimal(pawn, animal, out text, forced, this.canInteractWhileSleeping, false))
			{
				return true;
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			return false;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x00136DD8 File Offset: 0x00134FD8
		public static bool CanInteractWithAnimal(Pawn pawn, Pawn animal, out string jobFailReason, bool forced, bool canInteractWhileSleeping = false, bool ignoreSkillRequirements = false)
		{
			jobFailReason = null;
			if (!pawn.CanReserve(animal, 1, -1, null, forced))
			{
				return false;
			}
			if (animal.Downed)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans;
				return false;
			}
			if (!animal.Awake() && !canInteractWhileSleeping)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans;
				return false;
			}
			if (!animal.CanCasuallyInteractNow(false, canInteractWhileSleeping))
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans;
				return false;
			}
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			if (!ignoreSkillRequirements && num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				jobFailReason = "AnimalsSkillTooLow".Translate(num);
				return false;
			}
			return true;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00136E74 File Offset: 0x00135074
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

		// Token: 0x060036DB RID: 14043 RVA: 0x00136F28 File Offset: 0x00135128
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined, new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f));
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

		// Token: 0x04001EFD RID: 7933
		protected static string NoUsableFoodTrans;

		// Token: 0x04001EFE RID: 7934
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x04001EFF RID: 7935
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x04001F00 RID: 7936
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x04001F01 RID: 7937
		private static string CantInteractAnimalBusyTrans;

		// Token: 0x04001F02 RID: 7938
		protected bool canInteractWhileSleeping;
	}
}
