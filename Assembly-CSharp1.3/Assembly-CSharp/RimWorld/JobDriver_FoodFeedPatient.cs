using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000751 RID: 1873
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x000FE409 File Offset: 0x000FC609
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x001263A4 File Offset: 0x001245A4
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x00126520 File Offset: 0x00124720
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x00126598 File Offset: 0x00124798
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
			{
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(this.Food, this.pawn, this.job.count);
				if (!this.pawn.Reserve(this.Food, this.job, 10, maxAmountToPickup, null, errorOnFailed))
				{
					return false;
				}
				this.job.count = maxAmountToPickup;
			}
			return true;
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x0012664A File Offset: 0x0012484A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(this.Deliveree));
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}

		// Token: 0x04001E1D RID: 7709
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x04001E1E RID: 7710
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x04001E1F RID: 7711
		private const float FeedDurationMultiplier = 1.5f;
	}
}
