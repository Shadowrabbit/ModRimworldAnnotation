using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039B RID: 923
	public static class DebugActionsMapManagement
	{
		// Token: 0x06001B44 RID: 6980 RVA: 0x0009E414 File Offset: 0x0009C614
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMap()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0009E46C File Offset: 0x0009C66C
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void DestroyMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					Current.Game.DeinitAndRemoveMap(map);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0009E4DC File Offset: 0x0009C6DC
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void LeakMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					DebugActionsMapManagement.mapLeak = map;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0009E54C File Offset: 0x0009C74C
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak));
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0009E562 File Offset: 0x0009C762
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void AddGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Add_GameCondition()));
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0009E578 File Offset: 0x0009C778
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RemoveGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Remove_GameCondition()));
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0009E590 File Offset: 0x0009C790
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing, actionType = DebugActionType.ToolMap)]
		private static void Transfer()
		{
			List<Thing> toTransfer = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (!toTransfer.Any<Thing>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					Predicate<IntVec3> <>9__1;
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						for (int j = 0; j < toTransfer.Count; j++)
						{
							Map map;
							IntVec3 center = map.Center;
							map = map;
							int squareRadius = Mathf.Max(map.Size.x, map.Size.z);
							Predicate<IntVec3> validator;
							if ((validator = <>9__1) == null)
							{
								validator = (<>9__1 = ((IntVec3 x) => !x.Fogged(map) && x.Standable(map)));
							}
							IntVec3 center2;
							if (CellFinder.TryFindRandomCellNear(center, map, squareRadius, validator, out center2, -1))
							{
								toTransfer[j].DeSpawn(DestroyMode.Vanish);
								GenPlace.TryPlaceThing(toTransfer[j], center2, map, ThingPlaceMode.Near, null, null, default(Rot4));
							}
							else
							{
								Log.Error("Could not find spawn cell.");
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0009E650 File Offset: 0x0009C850
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void ChangeMap()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map != Find.CurrentMap)
				{
					list.Add(new DebugMenuOption(map.ToString(), DebugMenuOptionMode.Action, delegate()
					{
						Current.Game.CurrentMap = map;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0009E6D0 File Offset: 0x0009C8D0
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RegenerateCurrentMap()
		{
			RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
			int tile = Find.CurrentMap.Tile;
			MapParent parent = Find.CurrentMap.Parent;
			IntVec3 size = Find.CurrentMap.Size;
			Current.Game.DeinitAndRemoveMap(Find.CurrentMap);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, parent.def);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0009E758 File Offset: 0x0009C958
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMapWithCaves()
		{
			int tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, false, (int x) => Find.World.HasCaves(x));
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.Parent.Destroy();
			}
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, null);
			Current.Game.CurrentMap = orGenerateMap;
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0009E808 File Offset: 0x0009CA08
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RunMapGenerator()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<MapGeneratorDef>.Enumerator enumerator = DefDatabase<MapGeneratorDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MapGeneratorDef mapgen = enumerator.Current;
					list.Add(new DebugMenuOption(mapgen.defName, DebugMenuOptionMode.Action, delegate()
					{
						MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
						mapParent.Tile = (from tile in Enumerable.Range(0, Find.WorldGrid.TilesCount)
						where Find.WorldGrid[tile].biome.canBuildBase
						select tile).RandomElement<int>();
						mapParent.SetFaction(Faction.OfPlayer);
						Find.WorldObjects.Add(mapParent);
						Map currentMap = MapGenerator.GenerateMap(Find.World.info.initialMapSize, mapParent, mapgen, null, null);
						Current.Game.CurrentMap = currentMap;
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0009E898 File Offset: 0x0009CA98
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void ForceReformInCurrentMap()
		{
			if (Find.CurrentMap != null)
			{
				MapParent mapParent = Find.CurrentMap.Parent;
				List<Pawn> list = new List<Pawn>();
				if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
				{
					Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
					Current.Game.CurrentMap = mapParent.Map;
					Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
					{
						if (mapParent.HasMap)
						{
							mapParent.Destroy();
						}
					}, true);
					Find.WindowStack.Add(window);
					return;
				}
				list.Clear();
				list.AddRange(from x in mapParent.Map.mapPawns.AllPawns
				where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
				select x);
				if (list.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
				{
					CaravanExitMapUtility.ExitMapAndCreateCaravan(list, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, -1, true);
				}
				list.Clear();
				mapParent.Destroy();
			}
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0009EA14 File Offset: 0x0009CC14
		public static List<DebugMenuOption> Options_Add_GameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				Action <>9__1;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "Permanent";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
					Action method;
					if ((method = <>9__1) == null)
					{
						method = (<>9__1 = delegate()
						{
							GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
							gameCondition.Permanent = true;
							Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					for (int i = 2500; i <= 60000; i += 2500)
					{
						int localTicks = i;
						list2.Add(new DebugMenuOption(localTicks.ToStringTicksToPeriod(true, false, true, true) ?? "", DebugMenuOptionMode.Action, delegate()
						{
							GameCondition gameCondition = GameConditionMaker.MakeCondition(localDef, -1);
							gameCondition.Duration = localTicks;
							Find.CurrentMap.GameConditionManager.RegisterCondition(gameCondition);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			return list;
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0009EA98 File Offset: 0x0009CC98
		public static List<DebugMenuOption> Options_Remove_GameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameCondition localCondition2 in Find.CurrentMap.gameConditionManager.ActiveConditions)
			{
				GameCondition localCondition = localCondition2;
				list.Add(new DebugMenuOption(localCondition.def.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					localCondition.Duration = 0;
				}));
			}
			return list;
		}

		// Token: 0x0400119D RID: 4509
		private static Map mapLeak;
	}
}
