using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public class JobDriver_FoodDeliver : JobDriver
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x001263A4 File Offset: 0x001245A4
		private Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x001263F2 File Offset: 0x001245F2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x00126420 File Offset: 0x00124620
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x00126498 File Offset: 0x00124698
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x001264EB File Offset: 0x001246EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x0012650D File Offset: 0x0012470D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (this.usingNutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.A, this.Deliveree);
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			Toil toil2 = new Toil();
			toil2.initAction = delegate()
			{
				Pawn actor = toil2.actor;
				Job curJob = actor.jobs.curJob;
				actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil2.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			toil2.AddFailCondition(delegate
			{
				Pawn pawn = (Pawn)toil2.actor.jobs.curJob.targetB.Thing;
				return !pawn.IsPrisonerOfColony || !pawn.guest.CanBeBroughtFood;
			});
			yield return toil2;
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, null);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x04001E19 RID: 7705
		private bool usingNutrientPasteDispenser;

		// Token: 0x04001E1A RID: 7706
		private bool eatingFromInventory;

		// Token: 0x04001E1B RID: 7707
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x04001E1C RID: 7708
		private const TargetIndex DelivereeInd = TargetIndex.B;
	}
}
