using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200098A RID: 2442
	public class JobDriver_DoBill : JobDriver
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x0002DAD2 File Offset: 0x0002BCD2
		public override string GetReport()
		{
			if (this.job.RecipeDef != null)
			{
				return this.ReportStringProcessed(this.job.RecipeDef.jobString);
			}
			return base.GetReport();
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x0016E7B0 File Offset: 0x0016C9B0
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

		// Token: 0x06003BBA RID: 15290 RVA: 0x0016E7E4 File Offset: 0x0016C9E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0016E834 File Offset: 0x0016CA34
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0002DAFE File Offset: 0x0002BCFE
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
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return extract;
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, true, false, true);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDestroyedOrNull(TargetIndex.B);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
			extract = null;
			getToHaulTarget = null;
			findPlaceTarget = null;
			yield return gotoBillGiver;
			yield return Toils_Recipe.MakeUnfinishedThingIfNeeded();
			yield return Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings().FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Recipe.FinishRecipeAndStartStoringProduct();
			if (!this.job.RecipeDef.products.NullOrEmpty<ThingDefCountClass>() || !this.job.RecipeDef.specialProducts.NullOrEmpty<SpecialProductType>())
			{
				JobDriver_DoBill.<>c__DisplayClass12_0 CS$<>8__locals1 = new JobDriver_DoBill.<>c__DisplayClass12_0();
				CS$<>8__locals1.<>4__this = this;
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				findPlaceTarget = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return findPlaceTarget;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, findPlaceTarget, true, true);
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
				findPlaceTarget = null;
			}
			yield break;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0016E888 File Offset: 0x0016CA88
		private static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.", false);
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

		// Token: 0x0400295A RID: 10586
		public float workLeft;

		// Token: 0x0400295B RID: 10587
		public int billStartTick;

		// Token: 0x0400295C RID: 10588
		public int ticksSpentDoingRecipeWork;

		// Token: 0x0400295D RID: 10589
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		// Token: 0x0400295E RID: 10590
		public const TargetIndex BillGiverInd = TargetIndex.A;

		// Token: 0x0400295F RID: 10591
		public const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04002960 RID: 10592
		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;
	}
}
