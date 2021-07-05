using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C4 RID: 1732
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanInteractNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x0011D572 File Offset: 0x0011B772
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x0011D590 File Offset: 0x0011B790
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x0011D5B2 File Offset: 0x0011B7B2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			foreach (Toil toil in this.FeedToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			foreach (Toil toil2 in this.FeedToils())
			{
				yield return toil2;
			}
			enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.CanInteractNow);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield break;
			yield break;
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x0011D5C2 File Offset: 0x0011B7C2
		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x0011D5E4 File Offset: 0x0011B7E4
		private IEnumerable<Toil> FeedToils()
		{
			yield return new Toil
			{
				initAction = delegate()
				{
					this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(this.Animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoAnimal;
			yield return this.StartFeedAnimal(TargetIndex.A);
			yield return Toils_Ingest.FinalizeIngest(this.Animal, TargetIndex.B);
			yield return Toils_General.PutCarriedThingInInventory();
			yield return Toils_General.ClearTarget(TargetIndex.B);
			yield return Toils_Jump.JumpIf(gotoAnimal, () => this.feedNutritionLeft > 0f);
			yield break;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x0011D5F4 File Offset: 0x0011B7F4
		private Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.FailOn(() => !this.CanInteractNow);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x0011D69C File Offset: 0x0011B89C
		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true));
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true);
				this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					this.feedNutritionLeft = 0f;
					return;
				}
				this.feedNutritionLeft -= num2;
				if (this.feedNutritionLeft < 0.001f)
				{
					this.feedNutritionLeft = 0f;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x04001D3F RID: 7487
		protected const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04001D40 RID: 7488
		private const TargetIndex FoodHandInd = TargetIndex.B;

		// Token: 0x04001D41 RID: 7489
		private const int FeedDuration = 270;

		// Token: 0x04001D42 RID: 7490
		private const int TalkDuration = 270;

		// Token: 0x04001D43 RID: 7491
		private const float NutritionPercentagePerFeed = 0.15f;

		// Token: 0x04001D44 RID: 7492
		private const float MaxMinNutritionPerFeed = 0.3f;

		// Token: 0x04001D45 RID: 7493
		public const int FeedCount = 2;

		// Token: 0x04001D46 RID: 7494
		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		// Token: 0x04001D47 RID: 7495
		private float feedNutritionLeft;
	}
}
