using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001486 RID: 5254
	public static class GenLeaving
	{
		// Token: 0x06007DA6 RID: 32166 RVA: 0x002C70A6 File Offset: 0x002C52A6
		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, List<Thing> listOfLeavingsOut = null)
		{
			GenLeaving.DoLeavingsFor(diedThing, map, mode, diedThing.OccupiedRect(), null, listOfLeavingsOut);
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x002C70B8 File Offset: 0x002C52B8
		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, CellRect leavingsRect, Predicate<IntVec3> nearPlaceValidator = null, List<Thing> listOfLeavingsOut = null)
		{
			if ((Current.ProgramState != ProgramState.Playing && mode != DestroyMode.Refund) || mode == DestroyMode.Vanish || mode == DestroyMode.QuestLogic)
			{
				return;
			}
			if (mode == DestroyMode.KillFinalize && diedThing.def.filthLeaving != null)
			{
				for (int i = leavingsRect.minZ; i <= leavingsRect.maxZ; i++)
				{
					for (int j = leavingsRect.minX; j <= leavingsRect.maxX; j++)
					{
						FilthMaker.TryMakeFilth(new IntVec3(j, 0, i), map, diedThing.def.filthLeaving, Rand.RangeInclusive(1, 3), FilthSourceFlags.None);
					}
				}
			}
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			if (mode == DestroyMode.KillFinalize && diedThing.def.killedLeavings != null)
			{
				for (int k = 0; k < diedThing.def.killedLeavings.Count; k++)
				{
					Thing thing = ThingMaker.MakeThing(diedThing.def.killedLeavings[k].thingDef, null);
					thing.stackCount = diedThing.def.killedLeavings[k].count;
					thingOwner.TryAdd(thing, true);
				}
			}
			if (GenLeaving.CanBuildingLeaveResources(diedThing, mode))
			{
				Frame frame = diedThing as Frame;
				if (frame != null)
				{
					for (int l = frame.resourceContainer.Count - 1; l >= 0; l--)
					{
						int num = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(frame.resourceContainer[l].stackCount);
						if (num > 0)
						{
							frame.resourceContainer.TryTransferToContainer(frame.resourceContainer[l], thingOwner, num, true);
						}
					}
					frame.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				else
				{
					List<ThingDefCountClass> list = diedThing.CostListAdjusted();
					for (int m = 0; m < list.Count; m++)
					{
						ThingDefCountClass thingDefCountClass = list[m];
						int num2 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(thingDefCountClass.count);
						if (num2 > 0 && mode == DestroyMode.KillFinalize && thingDefCountClass.thingDef.slagDef != null)
						{
							int count = thingDefCountClass.thingDef.slagDef.smeltProducts.First((ThingDefCountClass pro) => pro.thingDef == ThingDefOf.Steel).count;
							int num3 = num2 / count;
							num3 = Mathf.Min(num3, diedThing.def.Size.Area / 2);
							for (int n = 0; n < num3; n++)
							{
								thingOwner.TryAdd(ThingMaker.MakeThing(thingDefCountClass.thingDef.slagDef, null), true);
							}
							num2 -= num3 * count;
						}
						if (num2 > 0)
						{
							Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
							thing2.stackCount = num2;
							thingOwner.TryAdd(thing2, true);
						}
					}
				}
			}
			List<IntVec3> list2 = leavingsRect.Cells.InRandomOrder(null).ToList<IntVec3>();
			int num4 = 0;
			while (thingOwner.Count > 0)
			{
				if (mode == DestroyMode.KillFinalize && !map.areaManager.Home[list2[num4]])
				{
					thingOwner[0].SetForbidden(true, false);
				}
				Thing item;
				if (!thingOwner.TryDrop(thingOwner[0], list2[num4], map, ThingPlaceMode.Near, out item, null, nearPlaceValidator))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Failed to place all leavings for destroyed thing ",
						diedThing,
						" at ",
						leavingsRect.CenterCell
					}));
					return;
				}
				if (listOfLeavingsOut != null)
				{
					listOfLeavingsOut.Add(item);
				}
				num4++;
				if (num4 >= list2.Count)
				{
					num4 = 0;
				}
			}
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x002C7428 File Offset: 0x002C5628
		public static void DoLeavingsFor(TerrainDef terrain, IntVec3 cell, Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			List<ThingDefCountClass> list = terrain.CostListAdjusted(null, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = GenMath.RoundRandom((float)thingDefCountClass.count * terrain.resourcesFractionWhenDeconstructed);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing.stackCount = num;
					thingOwner.TryAdd(thing, true);
				}
			}
			while (thingOwner.Count > 0)
			{
				Thing thing2;
				if (!thingOwner.TryDrop(thingOwner[0], cell, map, ThingPlaceMode.Near, out thing2, null, null))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Failed to place all leavings for removed terrain ",
						terrain,
						" at ",
						cell
					}));
					return;
				}
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x002C74F0 File Offset: 0x002C56F0
		public static bool CanBuildingLeaveResources(Thing destroyedThing, DestroyMode mode)
		{
			if (!(destroyedThing is Building))
			{
				return false;
			}
			if (mode == DestroyMode.Deconstruct && typeof(Frame).IsAssignableFrom(destroyedThing.GetType()))
			{
				mode = DestroyMode.Cancel;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return false;
			case DestroyMode.WillReplace:
				return false;
			case DestroyMode.KillFinalize:
				return destroyedThing.def.leaveResourcesWhenKilled;
			case DestroyMode.Deconstruct:
				return destroyedThing.def.resourcesFractionWhenDeconstructed != 0f;
			case DestroyMode.FailConstruction:
				return true;
			case DestroyMode.Cancel:
				return true;
			case DestroyMode.Refund:
				return true;
			case DestroyMode.QuestLogic:
				return false;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode);
			}
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x002C7590 File Offset: 0x002C5790
		private static Func<int, int> GetBuildingResourcesLeaveCalculator(Thing destroyedThing, DestroyMode mode)
		{
			if (!GenLeaving.CanBuildingLeaveResources(destroyedThing, mode))
			{
				return (int count) => 0;
			}
			if (mode == DestroyMode.Deconstruct && typeof(Frame).IsAssignableFrom(destroyedThing.GetType()))
			{
				mode = DestroyMode.Cancel;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return (int count) => 0;
			case DestroyMode.WillReplace:
				return (int count) => 0;
			case DestroyMode.KillFinalize:
				return (int count) => GenMath.RoundRandom((float)count * 0.25f);
			case DestroyMode.Deconstruct:
				return (int count) => Mathf.Min(GenMath.RoundRandom((float)count * destroyedThing.def.resourcesFractionWhenDeconstructed), count);
			case DestroyMode.FailConstruction:
				return (int count) => GenMath.RoundRandom((float)count * 0.5f);
			case DestroyMode.Cancel:
				return (int count) => GenMath.RoundRandom((float)count * 1f);
			case DestroyMode.Refund:
				return (int count) => count;
			case DestroyMode.QuestLogic:
				return (int count) => 0;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode);
			}
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x002C7728 File Offset: 0x002C5928
		public static void DropFilthDueToDamage(Thing t, float damageDealt)
		{
			if (!t.def.useHitPoints || !t.Spawned || t.def.filthLeaving == null)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect().ExpandedBy(1);
			GenLeaving.tmpCellsCandidates.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				if (intVec.InBounds(t.Map) && intVec.Walkable(t.Map))
				{
					GenLeaving.tmpCellsCandidates.Add(intVec);
				}
			}
			if (!GenLeaving.tmpCellsCandidates.Any<IntVec3>())
			{
				return;
			}
			int num = GenMath.RoundRandom(damageDealt * Mathf.Min(0.016666668f, 1f / ((float)t.MaxHitPoints / 10f)));
			for (int i = 0; i < num; i++)
			{
				FilthMaker.TryMakeFilth(GenLeaving.tmpCellsCandidates.RandomElement<IntVec3>(), t.Map, t.def.filthLeaving, 1, FilthSourceFlags.None);
			}
			GenLeaving.tmpCellsCandidates.Clear();
		}

		// Token: 0x04004E5C RID: 20060
		private const float LeaveFraction_Kill = 0.25f;

		// Token: 0x04004E5D RID: 20061
		private const float LeaveFraction_Cancel = 1f;

		// Token: 0x04004E5E RID: 20062
		public const float LeaveFraction_DeconstructDefault = 0.5f;

		// Token: 0x04004E5F RID: 20063
		private const float LeaveFraction_FailConstruction = 0.5f;

		// Token: 0x04004E60 RID: 20064
		private static List<IntVec3> tmpCellsCandidates = new List<IntVec3>();
	}
}
