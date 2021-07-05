using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000858 RID: 2136
	public static class RefuelWorkGiverUtility
	{
		// Token: 0x0600386F RID: 14447 RVA: 0x0013D13C File Offset: 0x0013B33C
		public static bool CanRefuel(Pawn pawn, Thing t, bool forced = false)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			if (compRefuelable == null || compRefuelable.IsFull || (!forced && !compRefuelable.allowAutoRefuel))
			{
				return false;
			}
			if (compRefuelable.FuelPercentOfMax > 0f && !compRefuelable.Props.allowRefuelIfNotEmpty)
			{
				return false;
			}
			if (!forced && !compRefuelable.ShouldAutoRefuelNow)
			{
				return false;
			}
			if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			if (RefuelWorkGiverUtility.FindBestFuel(pawn, t) == null)
			{
				ThingFilter fuelFilter = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
				JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter.Summary), null);
				return false;
			}
			if (t.TryGetComp<CompRefuelable>().Props.atomicFueling && RefuelWorkGiverUtility.FindAllFuel(pawn, t) == null)
			{
				ThingFilter fuelFilter2 = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
				JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter2.Summary), null);
				return false;
			}
			return true;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0013D24C File Offset: 0x0013B44C
		public static Job RefuelJob(Pawn pawn, Thing t, bool forced = false, JobDef customRefuelJob = null, JobDef customAtomicRefuelJob = null)
		{
			if (!t.TryGetComp<CompRefuelable>().Props.atomicFueling)
			{
				Thing t2 = RefuelWorkGiverUtility.FindBestFuel(pawn, t);
				return JobMaker.MakeJob(customRefuelJob ?? JobDefOf.Refuel, t, t2);
			}
			List<Thing> source = RefuelWorkGiverUtility.FindAllFuel(pawn, t);
			Job job = JobMaker.MakeJob(customAtomicRefuelJob ?? JobDefOf.RefuelAtomic, t);
			job.targetQueueB = (from f in source
			select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
			return job;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0013D2E0 File Offset: 0x0013B4E0
		private static Thing FindBestFuel(Pawn pawn, Thing refuelable)
		{
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false) && filter.Allows(x);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, filter.BestThingRequest, PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0013D364 File Offset: 0x0013B564
		private static List<Thing> FindAllFuel(Pawn pawn, Thing refuelable)
		{
			int fuelCountToFullyRefuel = refuelable.TryGetComp<CompRefuelable>().GetFuelCountToFullyRefuel();
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, refuelable.Position, new IntRange(fuelCountToFullyRefuel, fuelCountToFullyRefuel), (Thing t) => filter.Allows(t));
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x0013D3B8 File Offset: 0x0013B5B8
		public static List<Thing> FindEnoughReservableThings(Pawn pawn, IntVec3 rootCell, IntRange desiredQuantity, Predicate<Thing> validThing)
		{
			RefuelWorkGiverUtility.<>c__DisplayClass4_0 CS$<>8__locals1 = new RefuelWorkGiverUtility.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.validThing = validThing;
			CS$<>8__locals1.desiredQuantity = desiredQuantity;
			CS$<>8__locals1.validator = ((Thing x) => !x.IsForbidden(CS$<>8__locals1.pawn) && CS$<>8__locals1.pawn.CanReserve(x, 1, -1, null, false) && CS$<>8__locals1.validThing(x));
			Region region = rootCell.GetRegion(CS$<>8__locals1.pawn.Map, RegionType.Set_Passable);
			CS$<>8__locals1.traverseParams = TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(CS$<>8__locals1.traverseParams, false);
			CS$<>8__locals1.chosenThings = new List<Thing>();
			CS$<>8__locals1.accumulatedQuantity = 0;
			CS$<>8__locals1.<FindEnoughReservableThings>g__ThingListProcessor|2(rootCell.GetThingList(region.Map), region);
			if (CS$<>8__locals1.accumulatedQuantity < CS$<>8__locals1.desiredQuantity.max)
			{
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, new RegionProcessor(CS$<>8__locals1.<FindEnoughReservableThings>g__RegionProcessor|3), 99999, RegionType.Set_Passable);
			}
			if (CS$<>8__locals1.accumulatedQuantity >= CS$<>8__locals1.desiredQuantity.min)
			{
				return CS$<>8__locals1.chosenThings;
			}
			return null;
		}
	}
}
