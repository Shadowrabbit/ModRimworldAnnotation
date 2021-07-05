﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200085D RID: 2141
	public static class GenPlace
	{
		// Token: 0x06003584 RID: 13700 RVA: 0x00158C38 File Offset: 0x00156E38
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, Rot4 rot = default(Rot4))
		{
			Thing thing2;
			return GenPlace.TryPlaceThing(thing, center, map, mode, out thing2, placedAction, nearPlaceValidator, rot);
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x00158C58 File Offset: 0x00156E58
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, Rot4 rot = default(Rot4))
		{
			if (map == null)
			{
				Log.Error("Tried to place thing " + thing + " in a null map.", false);
				lastResultingThing = null;
				return false;
			}
			if (thing.def.category == ThingCategory.Filth)
			{
				mode = ThingPlaceMode.Direct;
			}
			if (mode == ThingPlaceMode.Direct)
			{
				return GenPlace.TryPlaceDirect(thing, center, rot, map, out lastResultingThing, placedAction);
			}
			if (mode == ThingPlaceMode.Near)
			{
				lastResultingThing = null;
				for (;;)
				{
					int stackCount = thing.stackCount;
					IntVec3 loc;
					if (!GenPlace.TryFindPlaceSpotNear(center, rot, map, thing, true, out loc, nearPlaceValidator))
					{
						break;
					}
					if (GenPlace.TryPlaceDirect(thing, loc, rot, map, out lastResultingThing, placedAction))
					{
						return true;
					}
					if (thing.stackCount == stackCount)
					{
						goto Block_7;
					}
				}
				return false;
				Block_7:
				Log.Error(string.Concat(new object[]
				{
					"Failed to place ",
					thing,
					" at ",
					center,
					" in mode ",
					mode,
					"."
				}), false);
				lastResultingThing = null;
				return false;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x00158D3C File Offset: 0x00156F3C
		private static bool TryFindPlaceSpotNear(IntVec3 center, Rot4 rot, Map map, Thing thing, bool allowStacking, out IntVec3 bestSpot, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Unusable;
			bestSpot = center;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int j = 0; j < GenPlace.PlaceNearMiddleRadialCells; j++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[j];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				return true;
			}
			for (int k = 0; k < GenPlace.PlaceNearMaxRadialCells; k++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[k];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, rot, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			if (placeSpotQuality > GenPlace.PlaceSpotQuality.Unusable)
			{
				return true;
			}
			bestSpot = center;
			return false;
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00158E40 File Offset: 0x00157040
		private static GenPlace.PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Rot4 rot, Map map, Thing thing, IntVec3 center, bool allowStacking, Predicate<IntVec3> extraValidator = null)
		{
			if (!c.InBounds(map) || !c.Walkable(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (!GenAdj.OccupiedRect(c, rot, thing.def.Size).InBounds(map))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			if (extraValidator != null && !extraValidator(c))
			{
				return GenPlace.PlaceSpotQuality.Unusable;
			}
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing.def.saveCompressible && thing2.def.saveCompressible)
				{
					return GenPlace.PlaceSpotQuality.Unusable;
				}
				if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item && (!thing2.CanStackWith(thing) || thing2.stackCount >= thing.def.stackLimit))
				{
					return GenPlace.PlaceSpotQuality.Unusable;
				}
			}
			if (thing is Building)
			{
				foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, rot, thing.def.size))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && GenSpawn.SpawningWipes(thing.def, edifice.def))
					{
						return GenPlace.PlaceSpotQuality.Awful;
					}
				}
			}
			if (c.GetRoom(map, RegionType.Set_Passable) == center.GetRoom(map, RegionType.Set_Passable))
			{
				if (allowStacking)
				{
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing3 = list[j];
						if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing.def.stackLimit)
						{
							return GenPlace.PlaceSpotQuality.Perfect;
						}
					}
				}
				Pawn pawn = thing as Pawn;
				bool flag = pawn != null && pawn.Downed;
				GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Perfect;
				for (int k = 0; k < list.Count; k++)
				{
					Thing thing4 = list[k];
					if (thing4.def.IsDoor)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					if (thing4 is Building_WorkTable)
					{
						return GenPlace.PlaceSpotQuality.Bad;
					}
					Pawn pawn2 = thing4 as Pawn;
					if (pawn2 != null)
					{
						if (pawn2.Downed || flag)
						{
							return GenPlace.PlaceSpotQuality.Bad;
						}
						if (placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
						{
							placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
						}
					}
					if (thing4.def.category == ThingCategory.Plant && thing4.def.selectable && placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
					{
						placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
					}
				}
				return placeSpotQuality;
			}
			if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
			{
				return GenPlace.PlaceSpotQuality.Awful;
			}
			return GenPlace.PlaceSpotQuality.Bad;
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x001590B4 File Offset: 0x001572B4
		private static bool TryPlaceDirect(Thing thing, IntVec3 loc, Rot4 rot, Map map, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			Thing thing2 = thing;
			bool flag = false;
			if (thing.stackCount > thing.def.stackLimit)
			{
				thing = thing.SplitOff(thing.def.stackLimit);
				flag = true;
			}
			if (thing.def.stackLimit > 1)
			{
				List<Thing> thingList = loc.GetThingList(map);
				int i = 0;
				while (i < thingList.Count)
				{
					Thing thing3 = thingList[i];
					if (thing3.CanStackWith(thing))
					{
						int stackCount = thing.stackCount;
						if (thing3.TryAbsorbStack(thing, true))
						{
							resultingThing = thing3;
							if (placedAction != null)
							{
								placedAction(thing3, stackCount);
							}
							return !flag;
						}
						resultingThing = null;
						if (placedAction != null && stackCount != thing.stackCount)
						{
							placedAction(thing3, stackCount - thing.stackCount);
						}
						if (thing2 != thing)
						{
							thing2.TryAbsorbStack(thing, false);
						}
						return false;
					}
					else
					{
						i++;
					}
				}
			}
			resultingThing = GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false);
			if (placedAction != null)
			{
				placedAction(thing, thing.stackCount);
			}
			return !flag;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x001591B0 File Offset: 0x001573B0
		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprintsAndFrames)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (checkBlueprintsAndFrames && (thing.def.IsBlueprint || thing.def.IsFrame))
				{
					return thing;
				}
				if ((thing.def.category != ThingCategory.Plant || thing.def.passability != Traversability.Standable) && thing.def.category != ThingCategory.Filth && (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount))
				{
					if (thing.def.EverHaulable)
					{
						return thing;
					}
					if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def))
					{
						return thing;
					}
					if (thing.def.passability != Traversability.Standable && thing.def.surfaceType != SurfaceType.Item)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x0400253B RID: 9531
		private static readonly int PlaceNearMaxRadialCells = GenRadial.NumCellsInRadius(12.9f);

		// Token: 0x0400253C RID: 9532
		private static readonly int PlaceNearMiddleRadialCells = GenRadial.NumCellsInRadius(3f);

		// Token: 0x0200085E RID: 2142
		private enum PlaceSpotQuality : byte
		{
			// Token: 0x0400253E RID: 9534
			Unusable,
			// Token: 0x0400253F RID: 9535
			Awful,
			// Token: 0x04002540 RID: 9536
			Bad,
			// Token: 0x04002541 RID: 9537
			Okay,
			// Token: 0x04002542 RID: 9538
			Perfect
		}
	}
}
