﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001323 RID: 4899
	public static class MechClusterUtility
	{
		// Token: 0x060069F1 RID: 27121 RVA: 0x0020A5F0 File Offset: 0x002087F0
		public static IntVec3 FindClusterPosition(Map map, MechClusterSketch sketch, int maxTries = 100, float spawnCloseToColonyChance = 0f)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = float.MinValue;
			if (Rand.Chance(spawnCloseToColonyChance))
			{
				int num2 = 0;
				IntVec3 intVec;
				while (num2 < 20 && DropCellFinder.TryFindRaidDropCenterClose(out intVec, map, true, true, false, 40))
				{
					float clusterPositionScore = MechClusterUtility.GetClusterPositionScore(intVec, map, sketch);
					if (clusterPositionScore >= 100f || Mathf.Approximately(clusterPositionScore, 100f))
					{
						return intVec;
					}
					if (clusterPositionScore > num)
					{
						result = intVec;
						num = clusterPositionScore;
					}
					num2++;
				}
			}
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < maxTries; i++)
			{
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
				}
				IntVec3 intVec2 = CellFinderLoose.RandomCellWith(validator, map, 1000);
				if (intVec2.IsValid)
				{
					IntVec3 intVec3 = RCellFinder.FindSiegePositionFrom_NewTemp(intVec2, map, false);
					if (intVec3.IsValid)
					{
						float clusterPositionScore2 = MechClusterUtility.GetClusterPositionScore(intVec3, map, sketch);
						if (clusterPositionScore2 >= 100f || Mathf.Approximately(clusterPositionScore2, 100f))
						{
							return intVec3;
						}
						if (clusterPositionScore2 > num)
						{
							result = intVec2;
							num = clusterPositionScore2;
						}
					}
				}
			}
			if (!result.IsValid)
			{
				return CellFinder.RandomCell(map);
			}
			return result;
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x0020A72C File Offset: 0x0020892C
		public static float GetClusterPositionScore(IntVec3 center, Map map, MechClusterSketch sketch)
		{
			MechClusterUtility.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			if (sketch.buildingsSketch.AnyThingOutOfBounds(CS$<>8__locals1.map, center, Sketch.SpawnPosType.Unchanged))
			{
				return -100f;
			}
			if (sketch.pawns != null)
			{
				for (int i = 0; i < sketch.pawns.Count; i++)
				{
					if (!(sketch.pawns[i].position + center).InBounds(CS$<>8__locals1.map))
					{
						return -100f;
					}
				}
			}
			CS$<>8__locals1.colonyBuildings = CS$<>8__locals1.map.listerBuildings.allBuildingsColonist;
			CS$<>8__locals1.colonists = CS$<>8__locals1.map.mapPawns.FreeColonistsAndPrisonersSpawned;
			int num = 0;
			CS$<>8__locals1.fogged = 0;
			CS$<>8__locals1.roofed = 0;
			CS$<>8__locals1.indoors = 0;
			CS$<>8__locals1.tooCloseToColony = false;
			List<SketchEntity> entities = sketch.buildingsSketch.Entities;
			for (int j = 0; j < entities.Count; j++)
			{
				if (entities[j].IsSpawningBlocked(center, CS$<>8__locals1.map, null, false))
				{
					num++;
				}
				if (!MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(entities[j].pos + center, ref CS$<>8__locals1))
				{
					return -100f;
				}
			}
			if (sketch.pawns != null)
			{
				for (int k = 0; k < sketch.pawns.Count; k++)
				{
					if (!(sketch.pawns[k].position + center).Standable(CS$<>8__locals1.map))
					{
						num++;
					}
					if (!MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(sketch.pawns[k].position + center, ref CS$<>8__locals1))
					{
						return -100f;
					}
				}
			}
			int num2 = sketch.buildingsSketch.Entities.Count + ((sketch.pawns != null) ? sketch.pawns.Count : 0);
			float num3 = (float)num / (float)num2;
			float num4 = (float)CS$<>8__locals1.fogged / (float)num2;
			float a = (float)CS$<>8__locals1.roofed / (float)num2;
			float b = (float)CS$<>8__locals1.indoors / (float)num2;
			return 100f * (1f - num3) * (1f - Mathf.Max(a, b)) * (1f - num4) * (CS$<>8__locals1.tooCloseToColony ? 0.5f : 1f);
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x0000A2E4 File Offset: 0x000084E4
		[Obsolete]
		public static bool CanSpawnClusterAt(MechClusterSketch sketch, IntVec3 center, Map map, bool desperate = false)
		{
			return false;
		}

		// Token: 0x060069F4 RID: 27124 RVA: 0x00030CD3 File Offset: 0x0002EED3
		[Obsolete]
		public static IntVec3 FindDropPodLocation(Map map, Predicate<IntVec3> validator, int maxTries = 100, float spawnCloseToColonyChance = 0f)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x060069F5 RID: 27125 RVA: 0x000482F4 File Offset: 0x000464F4
		[Obsolete]
		private static bool TryFindRaidDropCenterClose(out IntVec3 result, Map map, Predicate<IntVec3> validator, int maxTries = 100)
		{
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060069F6 RID: 27126 RVA: 0x00030CD3 File Offset: 0x0002EED3
		[Obsolete]
		public static IntVec3 TryFindMechClusterPosInRect(CellRect rect, Map map, MechClusterSketch sketch)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x0020A95C File Offset: 0x00208B5C
		public static List<Thing> SpawnCluster(IntVec3 center, Map map, MechClusterSketch sketch, bool dropInPods = true, bool canAssaultColony = false, string questTag = null)
		{
			MechClusterUtility.<>c__DisplayClass14_0 CS$<>8__locals1 = new MechClusterUtility.<>c__DisplayClass14_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.sketch = sketch;
			foreach (IntVec3 a in CS$<>8__locals1.sketch.buildingsSketch.OccupiedRect)
			{
				IntVec3 c = a + center;
				if (c.InBounds(CS$<>8__locals1.map))
				{
					List<Thing> thingList = c.GetThingList(CS$<>8__locals1.map);
					Thing thing = null;
					foreach (Thing thing2 in thingList)
					{
						if (thing2.def.IsBlueprint)
						{
							thing = thing2;
							break;
						}
					}
					if (thing != null)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
			CS$<>8__locals1.spawnedThings = new List<Thing>();
			CS$<>8__locals1.spawnMode = (dropInPods ? Sketch.SpawnMode.TransportPod : Sketch.SpawnMode.Normal);
			CS$<>8__locals1.sketch.buildingsSketch.Spawn(CS$<>8__locals1.map, center, Faction.OfMechanoids, Sketch.SpawnPosType.Unchanged, CS$<>8__locals1.spawnMode, false, false, CS$<>8__locals1.spawnedThings, CS$<>8__locals1.sketch.startDormant, false, new Func<SketchEntity, IntVec3, bool>(CS$<>8__locals1.<SpawnCluster>g__CanSpawnThing|0), delegate(IntVec3 spot, SketchEntity entity)
			{
				SketchThing sketchThing;
				if ((sketchThing = (entity as SketchThing)) == null || sketchThing.def == ThingDefOf.Wall || sketchThing.def == ThingDefOf.Barricade)
				{
					return;
				}
				entity.SpawnNear_NewTmp(spot, CS$<>8__locals1.map, 12f, Faction.OfMechanoids, CS$<>8__locals1.spawnMode, false, CS$<>8__locals1.spawnedThings, CS$<>8__locals1.sketch.startDormant, new Func<SketchEntity, IntVec3, bool>(base.<SpawnCluster>g__CanSpawnThing|0));
			});
			float defendRadius = Mathf.Sqrt((float)(CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.x * CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.x + CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.z * CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.z)) / 2f + 6f;
			LordJob_MechanoidDefendBase lordJob_MechanoidDefendBase;
			if (CS$<>8__locals1.sketch.startDormant)
			{
				lordJob_MechanoidDefendBase = new LordJob_SleepThenMechanoidsDefend(CS$<>8__locals1.spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			else
			{
				lordJob_MechanoidDefendBase = new LordJob_MechanoidsDefend(CS$<>8__locals1.spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob_MechanoidDefendBase, CS$<>8__locals1.map, null);
			QuestUtility.AddQuestTag(lord, questTag);
			bool flag = Rand.Chance(0.6f);
			float randomInRange = MechClusterUtility.InitiationDelay.RandomInRange;
			int num = (int)(MechClusterUtility.MechAssemblerInitialDelayDays.RandomInRange * 60000f);
			for (int i = 0; i < CS$<>8__locals1.spawnedThings.Count; i++)
			{
				Thing thing3 = CS$<>8__locals1.spawnedThings[i];
				CompSpawnerPawn compSpawnerPawn = thing3.TryGetComp<CompSpawnerPawn>();
				if (compSpawnerPawn != null)
				{
					compSpawnerPawn.CalculateNextPawnSpawnTick((float)num);
				}
				if (thing3.TryGetComp<CompProjectileInterceptor>() != null)
				{
					lordJob_MechanoidDefendBase.AddThingToNotifyOnDefeat(thing3);
				}
				if (flag)
				{
					CompInitiatable compInitiatable = thing3.TryGetComp<CompInitiatable>();
					if (compInitiatable != null)
					{
						compInitiatable.initiationDelayTicksOverride = (int)(60000f * randomInRange);
					}
				}
				Building b;
				if ((b = (thing3 as Building)) != null && MechClusterUtility.IsBuildingThreat(b))
				{
					lord.AddBuilding(b);
				}
				thing3.SetFaction(Faction.OfMechanoids, null);
			}
			if (!CS$<>8__locals1.sketch.pawns.NullOrEmpty<MechClusterSketch.Mech>())
			{
				foreach (MechClusterSketch.Mech mech in CS$<>8__locals1.sketch.pawns)
				{
					IntVec3 intVec = mech.position + center;
					if (!intVec.Standable(CS$<>8__locals1.map))
					{
						IntVec3 root = intVec;
						Map map2 = CS$<>8__locals1.map;
						int squareRadius = 12;
						Predicate<IntVec3> validator;
						if ((validator = CS$<>8__locals1.<>9__2) == null)
						{
							validator = (CS$<>8__locals1.<>9__2 = ((IntVec3 x) => x.Standable(CS$<>8__locals1.map)));
						}
						if (!CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out intVec, -1))
						{
							continue;
						}
					}
					Pawn pawn = PawnGenerator.GeneratePawn(mech.kindDef, Faction.OfMechanoids);
					CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						if (CS$<>8__locals1.sketch.startDormant)
						{
							compCanBeDormant.ToSleep();
						}
						else
						{
							compCanBeDormant.WakeUp();
						}
					}
					lord.AddPawn(pawn);
					CS$<>8__locals1.spawnedThings.Add(pawn);
					if (dropInPods)
					{
						ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
						activeDropPodInfo.innerContainer.TryAdd(pawn, 1, true);
						activeDropPodInfo.openDelay = 60;
						activeDropPodInfo.leaveSlag = false;
						activeDropPodInfo.despawnPodBeforeSpawningThing = true;
						activeDropPodInfo.spawnWipeMode = new WipeMode?(WipeMode.Vanish);
						DropPodUtility.MakeDropPodAt(intVec, CS$<>8__locals1.map, activeDropPodInfo);
					}
					else
					{
						GenSpawn.Spawn(pawn, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
					}
				}
			}
			foreach (Thing thing4 in CS$<>8__locals1.spawnedThings)
			{
				if (!CS$<>8__locals1.sketch.startDormant)
				{
					CompWakeUpDormant compWakeUpDormant = thing4.TryGetComp<CompWakeUpDormant>();
					if (compWakeUpDormant != null)
					{
						compWakeUpDormant.Activate(true, true);
					}
				}
			}
			return CS$<>8__locals1.spawnedThings;
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x0020AE48 File Offset: 0x00209048
		private static bool IsBuildingThreat(Thing b)
		{
			CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = b.TryGetComp<CompPawnSpawnOnWakeup>();
			if (compPawnSpawnOnWakeup != null && compPawnSpawnOnWakeup.CanSpawn)
			{
				return true;
			}
			CompSpawnerPawn compSpawnerPawn = b.TryGetComp<CompSpawnerPawn>();
			return (compSpawnerPawn != null && compSpawnerPawn.pawnsLeftToSpawn != 0) || b.def.building.IsTurret || b.TryGetComp<CompCauseGameCondition>() != null;
		}

		// Token: 0x060069F9 RID: 27129 RVA: 0x0020AE9C File Offset: 0x0020909C
		public static bool AnyThreatBuilding(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Thing thing = things[i];
				if (thing is Building && !thing.Destroyed && MechClusterUtility.IsBuildingThreat(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x0020AEE0 File Offset: 0x002090E0
		[CompilerGenerated]
		internal static bool <GetClusterPositionScore>g__CheckCell|9_0(IntVec3 x, ref MechClusterUtility.<>c__DisplayClass9_0 A_1)
		{
			if (x.Fogged(A_1.map))
			{
				int num = A_1.fogged;
				A_1.fogged = num + 1;
			}
			if (x.Roofed(A_1.map))
			{
				int num = A_1.roofed;
				A_1.roofed = num + 1;
			}
			if (x.GetRoom(A_1.map, RegionType.Set_Passable) != null && !x.GetRoom(A_1.map, RegionType.Set_Passable).PsychologicallyOutdoors)
			{
				int num = A_1.indoors;
				A_1.indoors = num + 1;
			}
			using (List<Thing>.Enumerator enumerator = x.GetThingList(A_1.map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.preventSkyfallersLandingOn)
					{
						return false;
					}
				}
			}
			if (!A_1.tooCloseToColony)
			{
				for (int i = 0; i < A_1.colonyBuildings.Count; i++)
				{
					if (x.InHorDistOf(A_1.colonyBuildings[i].Position, 5f))
					{
						A_1.tooCloseToColony = true;
						break;
					}
				}
				if (!A_1.tooCloseToColony)
				{
					for (int j = 0; j < A_1.colonists.Count; j++)
					{
						if (x.InHorDistOf(A_1.colonists[j].Position, 5f))
						{
							A_1.tooCloseToColony = true;
							break;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04004690 RID: 18064
		private const int CloseToColonyRadius = 40;

		// Token: 0x04004691 RID: 18065
		private const int MinDistanceToColony = 5;

		// Token: 0x04004692 RID: 18066
		private const float RetrySpawnEntityRadius = 12f;

		// Token: 0x04004693 RID: 18067
		private const float MaxClusterPositionScore = 100f;

		// Token: 0x04004694 RID: 18068
		private const float InitiationChance = 0.6f;

		// Token: 0x04004695 RID: 18069
		private static readonly FloatRange InitiationDelay = new FloatRange(0.1f, 15f);

		// Token: 0x04004696 RID: 18070
		private static readonly FloatRange MechAssemblerInitialDelayDays = new FloatRange(0.5f, 1.5f);

		// Token: 0x04004697 RID: 18071
		public const string DefeatedSignal = "MechClusterDefeated";
	}
}
