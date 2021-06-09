using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000560 RID: 1376
	public static class DebugActionsMapManagement
	{
		// Token: 0x06002316 RID: 8982 RVA: 0x0010C438 File Offset: 0x0010A638
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void GenerateMap()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			mapParent.Tile = TileFinder.RandomStartingTile();
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0010C490 File Offset: 0x0010A690
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

		// Token: 0x06002318 RID: 8984 RVA: 0x0010C500 File Offset: 0x0010A700
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

		// Token: 0x06002319 RID: 8985 RVA: 0x0001DEBA File Offset: 0x0001C0BA
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void PrintLeakedMap()
		{
			Log.Message(string.Format("Leaked map {0}", DebugActionsMapManagement.mapLeak), false);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0001DED1 File Offset: 0x0001C0D1
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void AddGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Add_GameCondition()));
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0001DEE7 File Offset: 0x0001C0E7
		[DebugAction("Map management", null, allowedGameStates = AllowedGameStates.Playing)]
		private static void RemoveGameCondition()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugActionsMapManagement.Options_Remove_GameCondition()));
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0010C570 File Offset: 0x0010A770
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
								Log.Error("Could not find spawn cell.", false);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0010C630 File Offset: 0x0010A830
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

		// Token: 0x0600231E RID: 8990 RVA: 0x0010C6B0 File Offset: 0x0010A8B0
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

		// Token: 0x0600231F RID: 8991 RVA: 0x0010C738 File Offset: 0x0010A938
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

		// Token: 0x06002320 RID: 8992 RVA: 0x0010C7E8 File Offset: 0x0010A9E8
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

		// Token: 0x06002321 RID: 8993 RVA: 0x0010C878 File Offset: 0x0010AA78
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

		// Token: 0x06002322 RID: 8994 RVA: 0x0010C9F4 File Offset: 0x0010ABF4
		public static List<DebugMenuOption> Options_Add_GameCondition()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (GameConditionDef localDef2 in DefDatabase<GameConditionDef>.AllDefs)
			{
				GameConditionDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Find.CurrentMap.GameConditionManager.RegisterCondition(GameConditionMaker.MakeCondition(localDef, -1));
				}));
			}
			return list;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0010CA78 File Offset: 0x0010AC78
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

		// Token: 0x040017C2 RID: 6082
		private static Map mapLeak;
	}
}
