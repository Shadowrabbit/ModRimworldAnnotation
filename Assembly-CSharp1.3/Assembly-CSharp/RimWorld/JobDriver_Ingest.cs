﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000753 RID: 1875
	public class JobDriver_Ingest : JobDriver
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x00126725 File Offset: 0x00124925
		public bool EatingFromInventory
		{
			get
			{
				return this.eatingFromInventory;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x00126730 File Offset: 0x00124930
		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x00126754 File Offset: 0x00124954
		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					return 1f;
				}
				return 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x001267A4 File Offset: 0x001249A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x001267D0 File Offset: 0x001249D0
		public override string GetReport()
		{
			if (this.usingNutrientPasteDispenser)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, "", "", "", "");
			}
			Thing thing = this.job.targetA.Thing;
			if (thing != null && thing.def.ingestible != null)
			{
				if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || this.pawn.RaceProps.intelligence < Intelligence.ToolUser))
				{
					return thing.def.ingestible.ingestReportStringEat.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
				if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
				{
					return thing.def.ingestible.ingestReportString.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
			}
			return base.GetReport();
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x00126938 File Offset: 0x00124B38
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0012698C File Offset: 0x00124B8C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (!this.pawn.Reserve(ingestibleSource, this.job, 10, FoodUtility.GetMaxAmountToPickup(ingestibleSource, this.pawn, this.job.count), null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x001269F1 File Offset: 0x00124BF1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn(() => !this.IngestibleSource.Destroyed && !this.IngestibleSource.IngestibleNow);
			}
			Toil chew = Toils_Ingest.ChewIngestible(this.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !this.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != this.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			foreach (Toil toil in this.PrepareToIngestToils(chew))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return chew;
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(chew, () => this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
			yield break;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x00126A01 File Offset: 0x00124C01
		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (this.pawn.RaceProps.ToolUser)
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x00126A32 File Offset: 0x00124C32
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x00126A42 File Offset: 0x00124C42
		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else
			{
				yield return this.ReserveFood();
				Toil gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.JumpIf(gotoToPickup, () => this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.Jump(chewToil);
				yield return gotoToPickup;
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.pawn);
				gotoToPickup = null;
			}
			if (this.job.takeExtraIngestibles > 0)
			{
				foreach (Toil toil in this.TakeExtraIngestibles())
				{
					yield return toil;
				}
				IEnumerator<Toil> enumerator = null;
			}
			if (!this.pawn.Drafted)
			{
				yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			}
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
			yield break;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x00126A59 File Offset: 0x00124C59
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFood();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield break;
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00126A69 File Offset: 0x00124C69
		private IEnumerable<Toil> TakeExtraIngestibles()
		{
			if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield break;
			}
			Toil reserveExtraFoodToCollect = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.C, null);
			Toil findExtraFoodToCollect = new Toil();
			Predicate<Thing> <>9__2;
			findExtraFoodToCollect.initAction = delegate()
			{
				if (this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def) < this.job.takeExtraIngestibles)
				{
					IntVec3 position = this.pawn.Position;
					Map map = this.pawn.Map;
					ThingRequest thingReq = ThingRequest.ForDef(this.IngestibleSource.def);
					PathEndMode peMode = PathEndMode.Touch;
					TraverseParms traverseParams = TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
					float maxDistance = 30f;
					Predicate<Thing> validator;
					if ((validator = <>9__2) == null)
					{
						validator = (<>9__2 = ((Thing x) => this.pawn.CanReserve(x, 10, 1, null, false) && !x.IsForbidden(this.pawn) && x.IsSociallyProper(this.pawn)));
					}
					Thing thing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
					if (thing != null)
					{
						this.job.SetTarget(TargetIndex.C, thing);
						this.JumpToToil(reserveExtraFoodToCollect);
					}
				}
			};
			findExtraFoodToCollect.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_Jump.Jump(findExtraFoodToCollect);
			yield return reserveExtraFoodToCollect;
			yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
			yield return Toils_Haul.TakeToInventory(TargetIndex.C, () => this.job.takeExtraIngestibles - this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def));
			yield return findExtraFoodToCollect;
			yield break;
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x00126A79 File Offset: 0x00124C79
		private Toil ReserveFood()
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.Faction == null)
					{
						return;
					}
					Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.carryTracker.CarriedThing == thing)
					{
						return;
					}
					int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(thing, this.pawn, this.job.count);
					if (maxAmountToPickup == 0)
					{
						return;
					}
					if (!this.pawn.Reserve(thing, this.job, 10, maxAmountToPickup, null, true))
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn food reservation for ",
							this.pawn,
							" on job ",
							this,
							" failed, because it could not register food from ",
							thing,
							" - amount: ",
							maxAmountToPickup
						}));
						this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					}
					this.job.count = maxAmountToPickup;
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x00126AA0 File Offset: 0x00124CA0
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, this.pawn);
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x00126AD4 File Offset: 0x00124CD4
		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool behind, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing == null || !carriedThing.IngestibleNow)
			{
				return false;
			}
			if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
			{
				drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
				return true;
			}
			if (carriedThing.def.ingestible.ingestHoldOffsetStanding != null)
			{
				HoldOffset holdOffset = carriedThing.def.ingestible.ingestHoldOffsetStanding.Pick(pawn.Rotation);
				if (holdOffset != null)
				{
					drawPos += holdOffset.offset;
					behind = holdOffset.behind;
					flip = holdOffset.flip;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001E21 RID: 7713
		private bool usingNutrientPasteDispenser;

		// Token: 0x04001E22 RID: 7714
		private bool eatingFromInventory;

		// Token: 0x04001E23 RID: 7715
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		// Token: 0x04001E24 RID: 7716
		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		// Token: 0x04001E25 RID: 7717
		private const TargetIndex TableCellInd = TargetIndex.B;

		// Token: 0x04001E26 RID: 7718
		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;
	}
}
