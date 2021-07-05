using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005A3 RID: 1443
	public class JobDriver_DoBill : JobDriver
	{
		// Token: 0x060029FD RID: 10749 RVA: 0x000FD287 File Offset: 0x000FB487
		public override string GetReport()
		{
			if (this.job.RecipeDef != null)
			{
				return this.ReportStringProcessed(this.job.RecipeDef.jobString);
			}
			return base.GetReport();
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000FD2B4 File Offset: 0x000FB4B4
		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000FD2E8 File Offset: 0x000FB4E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000FD338 File Offset: 0x000FB538
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000FD3CC File Offset: 0x000FB5CC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (thing is Building && !thing.Spawned)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(delegate()
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (this.job.bill.DeletedOrDereferenced)
					{
						return true;
					}
					if (!billGiver.CurrentlyUsableForBills())
					{
						return true;
					}
				}
				return false;
			});
			Toil gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueB != null && this.job.targetQueueB.Count == 1)
					{
						UnfinishedThing unfinishedThing = this.job.targetQueueB[0].Thing as UnfinishedThing;
						if (unfinishedThing != null)
						{
							unfinishedThing.BoundBill = (Bill_ProductionWithUft)this.job.bill;
						}
					}
				}
			};
			yield return Toils_Jump.JumpIf(gotoBillGiver, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil in JobDriver_DoBill.CollectIngredientsToils(TargetIndex.B, TargetIndex.A, TargetIndex.C, false, true))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return gotoBillGiver;
			yield return Toils_Recipe.MakeUnfinishedThingIfNeeded();
			yield return Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings().FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Recipe.FinishRecipeAndStartStoringProduct();
			if (!this.job.RecipeDef.products.NullOrEmpty<ThingDefCountClass>() || !this.job.RecipeDef.specialProducts.NullOrEmpty<SpecialProductType>())
			{
				JobDriver_DoBill.<>c__DisplayClass12_0 CS$<>8__locals1 = new JobDriver_DoBill.<>c__DisplayClass12_0();
				CS$<>8__locals1.<>4__this = this;
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, true);
				CS$<>8__locals1.recount = new Toil();
				CS$<>8__locals1.recount.initAction = delegate()
				{
					Bill_Production bill_Production = CS$<>8__locals1.recount.actor.jobs.curJob.bill as Bill_Production;
					if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
					{
						CS$<>8__locals1.<>4__this.Map.resourceCounter.UpdateResourceCounts();
					}
				};
				yield return CS$<>8__locals1.recount;
				CS$<>8__locals1 = null;
				carryToCell = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000FD3DC File Offset: 0x000FB5DC
		public static IEnumerable<Toil> CollectIngredientsToils(TargetIndex ingredientInd, TargetIndex billGiverInd, TargetIndex ingredientPlaceCellInd, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = true)
		{
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(ingredientInd, true);
			yield return extract;
			Toil getToHaulTarget = Toils_Goto.GotoThing(ingredientInd, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(ingredientInd).FailOnSomeonePhysicallyInteracting(ingredientInd);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(ingredientInd, true, subtractNumTakenFromJobCount, failIfStackCountLessThanJobCount);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(billGiverInd, PathEndMode.InteractionCell).FailOnDestroyedOrNull(ingredientInd);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(billGiverInd, ingredientInd, ingredientPlaceCellInd);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(ingredientPlaceCellInd, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(ingredientInd, extract);
			yield break;
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000FD40C File Offset: 0x000FB60C
		public static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.");
					return;
				}
				if (actor.carryTracker.Full)
				{
					return;
				}
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing) && targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing) && (float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
					{
						int num = (actor.carryTracker.CarriedThing == null) ? 0 : actor.carryTracker.CarriedThing.stackCount;
						int num2 = curJob.countQueue[i];
						num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
						num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
						if (num2 > 0)
						{
							curJob.count = num2;
							curJob.SetTarget(ind, targetQueue[i].Thing);
							List<int> countQueue = curJob.countQueue;
							int index = i;
							countQueue[index] -= num2;
							if (curJob.countQueue[i] <= 0)
							{
								curJob.countQueue.RemoveAt(i);
								targetQueue.RemoveAt(i);
							}
							actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
							return;
						}
					}
				}
			};
			return toil;
		}

		// Token: 0x04001A18 RID: 6680
		public float workLeft;

		// Token: 0x04001A19 RID: 6681
		public int billStartTick;

		// Token: 0x04001A1A RID: 6682
		public int ticksSpentDoingRecipeWork;

		// Token: 0x04001A1B RID: 6683
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		// Token: 0x04001A1C RID: 6684
		public const TargetIndex BillGiverInd = TargetIndex.A;

		// Token: 0x04001A1D RID: 6685
		public const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04001A1E RID: 6686
		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;
	}
}
