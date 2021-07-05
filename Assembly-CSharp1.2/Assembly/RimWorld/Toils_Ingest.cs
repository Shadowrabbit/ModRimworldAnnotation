using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C5C RID: 3164
	public class Toils_Ingest
	{
		// Token: 0x06004A41 RID: 19009 RVA: 0x001A03F4 File Offset: 0x0019E5F4
		public static Toil TakeMealFromDispenser(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = ((Building_NutrientPasteDispenser)actor.jobs.curJob.GetTarget(ind).Thing).TryDispenseFood();
				if (thing == null)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				actor.carryTracker.TryStartCarry(thing);
				actor.CurJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.FailOnCannotTouch(ind, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = Building_NutrientPasteDispenser.CollectDuration;
			return toil;
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x001A0468 File Offset: 0x0019E668
		public static Toil PickupIngestible(TargetIndex ind, Pawn eater)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				if (curJob.count <= 0)
				{
					Log.Error("Tried to do PickupIngestible toil with job.maxNumToCarry = " + curJob.count, false);
					actor.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					return;
				}
				int count = Mathf.Min(thing.stackCount, curJob.count);
				actor.carryTracker.TryStartCarry(thing, count, true);
				if (thing != actor.carryTracker.CarriedThing && actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
				{
					actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				actor.jobs.curJob.targetA = actor.carryTracker.CarriedThing;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x001A04B8 File Offset: 0x0019E6B8
		public static Toil CarryIngestibleToChewSpot(Pawn pawn, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 intVec = IntVec3.Invalid;
				Thing thing = null;
				Thing thing2 = actor.CurJob.GetTarget(ingestibleInd).Thing;
				Predicate<Thing> baseChairValidator = delegate(Thing t)
				{
					if (t.def.building == null || !t.def.building.isSittable)
					{
						return false;
					}
					if (t.IsForbidden(pawn))
					{
						return false;
					}
					if (!actor.CanReserve(t, 1, -1, null, false))
					{
						return false;
					}
					if (!t.IsSociallyProper(actor))
					{
						return false;
					}
					if (t.IsBurning())
					{
						return false;
					}
					if (t.HostileTo(pawn))
					{
						return false;
					}
					bool result = false;
					for (int i = 0; i < 4; i++)
					{
						Building edifice = (t.Position + GenAdj.CardinalDirections[i]).GetEdifice(t.Map);
						if (edifice != null && edifice.def.surfaceType == SurfaceType.Eat)
						{
							result = true;
							break;
						}
					}
					return result;
				};
				if (thing2.def.ingestible.chairSearchRadius > 0f)
				{
					thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false);
				}
				if (thing == null)
				{
					intVec = RCellFinder.SpotToChewStandingNear(actor, actor.CurJob.GetTarget(ingestibleInd).Thing);
					Danger chewSpotDanger = intVec.GetDangerFor(pawn, actor.Map);
					if (chewSpotDanger != Danger.None)
					{
						thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), thing2.def.ingestible.chairSearchRadius, (Thing t) => baseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) <= chewSpotDanger, null, 0, -1, false, RegionType.Set_Passable, false);
					}
				}
				if (thing != null)
				{
					intVec = thing.Position;
					actor.Reserve(thing, actor.CurJob, 1, -1, null, true);
				}
				actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, intVec);
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x001A0510 File Offset: 0x0019E710
		public static Toil ReserveFoodFromStackForIngesting(TargetIndex ind, Pawn ingester = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (ingester == null)
				{
					ingester = toil.actor;
				}
				int stackCount = -1;
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing && target.Thing.Spawned && target.Thing.IngestibleNow)
				{
					int b = FoodUtility.WillIngestStackCountOf(ingester, target.Thing.def, target.Thing.GetStatValue(StatDefOf.Nutrition, true));
					stackCount = Mathf.Min(target.Thing.stackCount, b);
				}
				if (!toil.actor.Reserve(target, toil.actor.CurJob, 10, stackCount, null, true))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x001A0574 File Offset: 0x0019E774
		public static bool TryFindAdjacentIngestionPlaceSpot(IntVec3 root, ThingDef ingestibleDef, Pawn pawn, out IntVec3 placeSpot)
		{
			placeSpot = IntVec3.Invalid;
			Func<Thing, bool> <>9__0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirections[i];
				if (intVec.HasEatSurface(pawn.Map))
				{
					IEnumerable<Thing> source = pawn.Map.thingGrid.ThingsAt(intVec);
					Func<Thing, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Thing t) => t.def == ingestibleDef));
					}
					if (!source.Where(predicate).Any<Thing>() && !intVec.IsForbidden(pawn))
					{
						placeSpot = intVec;
						return true;
					}
				}
			}
			if (!placeSpot.IsValid)
			{
				Toils_Ingest.spotSearchList.Clear();
				Toils_Ingest.cardinals.Shuffle<IntVec3>();
				for (int j = 0; j < 4; j++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.cardinals[j]);
				}
				Toils_Ingest.diagonals.Shuffle<IntVec3>();
				for (int k = 0; k < 4; k++)
				{
					Toils_Ingest.spotSearchList.Add(Toils_Ingest.diagonals[k]);
				}
				Toils_Ingest.spotSearchList.Add(IntVec3.Zero);
				Func<Thing, bool> <>9__1;
				for (int l = 0; l < Toils_Ingest.spotSearchList.Count; l++)
				{
					IntVec3 intVec2 = root + Toils_Ingest.spotSearchList[l];
					if (intVec2.Walkable(pawn.Map) && !intVec2.IsForbidden(pawn))
					{
						IEnumerable<Thing> source2 = pawn.Map.thingGrid.ThingsAt(intVec2);
						Func<Thing, bool> predicate2;
						if ((predicate2 = <>9__1) == null)
						{
							predicate2 = (<>9__1 = ((Thing t) => t.def == ingestibleDef));
						}
						if (!source2.Where(predicate2).Any<Thing>())
						{
							placeSpot = intVec2;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x001A0728 File Offset: 0x0019E928
		public static Toil FindAdjacentEatSurface(TargetIndex eatSurfaceInd, TargetIndex foodInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 position = actor.Position;
				Map map = actor.Map;
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					IntVec3 intVec = position + rot.FacingCell;
					if (intVec.HasEatSurface(map))
					{
						toil.actor.CurJob.SetTarget(eatSurfaceInd, intVec);
						toil.actor.jobs.curDriver.rotateToFace = eatSurfaceInd;
						Thing thing = toil.actor.CurJob.GetTarget(foodInd).Thing;
						if (thing.def.rotatable)
						{
							thing.Rotation = Rot4.FromIntVec3(intVec - toil.actor.Position);
						}
						return;
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x001A0780 File Offset: 0x0019E980
		public static Toil ChewIngestible(Pawn chewer, float durationMultiplier, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (!thing.IngestibleNow)
				{
					chewer.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.pather.StopDead();
				actor.jobs.curDriver.ticksLeftThisToil = Mathf.RoundToInt((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
				if (thing.Spawned)
				{
					thing.Map.physicalInteractionReservationManager.Reserve(chewer, actor.CurJob, thing);
				}
			};
			toil.tickAction = delegate()
			{
				if (chewer != toil.actor)
				{
					toil.actor.rotationTracker.FaceCell(chewer.Position);
				}
				else
				{
					Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
					if (thing != null && thing.Spawned)
					{
						toil.actor.rotationTracker.FaceCell(thing.Position);
					}
					else if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
					{
						toil.actor.rotationTracker.FaceCell(toil.actor.CurJob.GetTarget(eatSurfaceInd).Cell);
					}
				}
				toil.actor.GainComfortFromCellIfPossible(false);
			};
			toil.WithProgressBar(ingestibleInd, delegate
			{
				Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (thing == null)
				{
					return 1f;
				}
				return 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / Mathf.Round((float)thing.def.ingestible.baseIngestTicks * durationMultiplier);
			}, false, -0.5f);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.FailOnDestroyedOrNull(ingestibleInd);
			toil.AddFinishAction(delegate
			{
				if (chewer == null)
				{
					return;
				}
				if (chewer.CurJob == null)
				{
					return;
				}
				Thing thing = chewer.CurJob.GetTarget(ingestibleInd).Thing;
				if (thing == null)
				{
					return;
				}
				if (chewer.Map.physicalInteractionReservationManager.IsReservedBy(chewer, thing))
				{
					chewer.Map.physicalInteractionReservationManager.Release(chewer, toil.actor.CurJob, thing);
				}
			});
			toil.handlingFacing = true;
			Toils_Ingest.AddIngestionEffects(toil, chewer, ingestibleInd, eatSurfaceInd);
			return toil;
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x001A0874 File Offset: 0x0019EA74
		public static Toil AddIngestionEffects(Toil toil, Pawn chewer, TargetIndex ingestibleInd, TargetIndex eatSurfaceInd)
		{
			toil.WithEffect(delegate()
			{
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target.HasThing)
				{
					return null;
				}
				EffecterDef result = target.Thing.def.ingestible.ingestEffect;
				if (chewer.RaceProps.intelligence < Intelligence.ToolUser && target.Thing.def.ingestible.ingestEffectEat != null)
				{
					result = target.Thing.def.ingestible.ingestEffectEat;
				}
				return result;
			}, delegate()
			{
				if (!toil.actor.CurJob.GetTarget(ingestibleInd).HasThing)
				{
					return null;
				}
				Thing thing = toil.actor.CurJob.GetTarget(ingestibleInd).Thing;
				if (chewer != toil.actor)
				{
					return chewer;
				}
				if (eatSurfaceInd != TargetIndex.None && toil.actor.CurJob.GetTarget(eatSurfaceInd).IsValid)
				{
					return toil.actor.CurJob.GetTarget(eatSurfaceInd);
				}
				return thing;
			});
			toil.PlaySustainerOrSound(delegate()
			{
				if (!chewer.RaceProps.Humanlike)
				{
					return null;
				}
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ingestibleInd);
				if (!target.HasThing)
				{
					return null;
				}
				return target.Thing.def.ingestible.ingestSound;
			});
			return toil;
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x001A08E8 File Offset: 0x0019EAE8
		public static Toil FinalizeIngest(Pawn ingester, TargetIndex ingestibleInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ingestibleInd).Thing;
				if (ingester.needs.mood != null && thing.def.IsNutritionGivingIngestible && thing.def.ingestible.chairSearchRadius > 10f)
				{
					if (!(ingester.Position + ingester.Rotation.FacingCell).HasEatSurface(actor.Map) && ingester.GetPosture() == PawnPosture.Standing && !ingester.IsWildMan())
					{
						ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AteWithoutTable, null);
					}
					Room room = ingester.GetRoom(RegionType.Set_Passable);
					if (room != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
						if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
						{
							ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.AteInImpressiveDiningRoom, scoreStageIndex), null);
						}
					}
				}
				float num = ingester.needs.food.NutritionWanted;
				if (curJob.overeat)
				{
					num = Mathf.Max(num, 0.75f);
				}
				float num2 = thing.Ingested(ingester, num);
				if (!ingester.Dead)
				{
					ingester.needs.food.CurLevel += num2;
				}
				ingester.records.AddTo(RecordDefOf.NutritionEaten, num2);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04003140 RID: 12608
		public const int MaxPawnReservations = 10;

		// Token: 0x04003141 RID: 12609
		private static List<IntVec3> spotSearchList = new List<IntVec3>();

		// Token: 0x04003142 RID: 12610
		private static List<IntVec3> cardinals = GenAdj.CardinalDirections.ToList<IntVec3>();

		// Token: 0x04003143 RID: 12611
		private static List<IntVec3> diagonals = GenAdj.DiagonalDirections.ToList<IntVec3>();
	}
}
