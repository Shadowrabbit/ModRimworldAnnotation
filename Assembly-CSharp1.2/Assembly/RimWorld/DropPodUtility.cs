using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001734 RID: 5940
	public static class DropPodUtility
	{
		// Token: 0x06008302 RID: 33538 RVA: 0x0026D560 File Offset: 0x0026B760
		public static void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info)
		{
			ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeDropPod.Contents = info;
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.DropPodIncoming, activeDropPod, c, map);
			using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)activeDropPod.Contents.innerContainer).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
						Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
						if (psychicEntropy != null)
						{
							psychicEntropy.SetInitialPsyfocusLevel();
						}
					}
				}
			}
		}

		// Token: 0x06008303 RID: 33539 RVA: 0x0026D600 File Offset: 0x0026B800
		public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true)
		{
			DropPodUtility.tempList.Clear();
			foreach (Thing item in things)
			{
				List<Thing> list = new List<Thing>();
				list.Add(item);
				DropPodUtility.tempList.Add(list);
			}
			DropPodUtility.DropThingGroupsNear(dropCenter, map, DropPodUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag, canRoofPunch, forbid);
			DropPodUtility.tempList.Clear();
		}

		// Token: 0x06008304 RID: 33540 RVA: 0x0026D684 File Offset: 0x0026B884
		public static void DropThingGroupsNear_NewTmp(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true)
		{
			Predicate<IntVec3> <>9__0;
			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, canRoofPunch, true, null) && (canRoofPunch || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, true, true, null)))
				{
					Log.Warning(string.Concat(new object[]
					{
						"DropThingsNear failed to find a place to drop ",
						list.FirstOrDefault<Thing>(),
						" near ",
						dropCenter,
						". Dropping on random square instead."
					}), false);
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => c.Walkable(map)));
					}
					intVec = CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
				if (forbid)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].SetForbidden(true, false);
					}
				}
				if (instaDrop)
				{
					using (List<Thing>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Thing thing = enumerator2.Current;
							GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Near, null, null, default(Rot4));
						}
						continue;
					}
				}
				ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
				foreach (Thing item in list)
				{
					activeDropPodInfo.innerContainer.TryAdd(item, true);
				}
				activeDropPodInfo.openDelay = openDelay;
				activeDropPodInfo.leaveSlag = leaveSlag;
				DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
			}
		}

		// Token: 0x06008305 RID: 33541 RVA: 0x0026D8A0 File Offset: 0x0026BAA0
		public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true)
		{
			DropPodUtility.DropThingGroupsNear_NewTmp(dropCenter, map, thingsGroups, openDelay, instaDrop, leaveSlag, canRoofPunch, forbid, true);
		}

		// Token: 0x040054F4 RID: 21748
		private static List<List<Thing>> tempList = new List<List<Thing>>();
	}
}
