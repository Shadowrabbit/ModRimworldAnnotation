using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017A6 RID: 6054
	public static class CaravanIncidentUtility
	{
		// Token: 0x06008C54 RID: 35924 RVA: 0x00325FB9 File Offset: 0x003241B9
		public static int CalculateIncidentMapSize(List<Pawn> caravanPawns, List<Pawn> enemies)
		{
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt((float)Mathf.RoundToInt((float)((caravanPawns.Count + enemies.Count) * 900)))), 75, 110);
		}

		// Token: 0x06008C55 RID: 35925 RVA: 0x00325FE8 File Offset: 0x003241E8
		public static bool CanFireIncidentWhichWantsToGenerateMapAt(int tile)
		{
			if (Current.Game.FindMap(tile) != null)
			{
				return false;
			}
			if (!Find.WorldGrid[tile].biome.implemented)
			{
				return false;
			}
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !allWorldObjects[i].def.allowCaravanIncidentsWhichGenerateMap)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008C56 RID: 35926 RVA: 0x00326060 File Offset: 0x00324260
		public static Map SetupCaravanAttackMap(Caravan caravan, List<Pawn> enemies, bool sendLetterIfRelatedPawns)
		{
			int num = CaravanIncidentUtility.CalculateIncidentMapSize(caravan.PawnsListForReading, enemies);
			Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(num, 1, num), WorldObjectDefOf.Ambush);
			IntVec3 playerStartingSpot;
			IntVec3 root;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerStartingSpot, out root);
			CaravanEnterMapUtility.Enter(caravan, map, (Pawn x) => CellFinder.RandomSpawnCellForPawnNear(playerStartingSpot, map, 4), CaravanDropInventoryMode.DoNotDrop, true);
			for (int i = 0; i < enemies.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 4);
				GenSpawn.Spawn(enemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
			}
			if (sendLetterIfRelatedPawns)
			{
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(enemies, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			return map;
		}

		// Token: 0x06008C57 RID: 35927 RVA: 0x0032613C File Offset: 0x0032433C
		public static Map GetOrGenerateMapForIncident(Caravan caravan, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			int tile = caravan.Tile;
			bool flag = Current.Game.FindMap(tile) == null;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, suggestedMapParentDef);
			if (flag && orGenerateMap != null)
			{
				orGenerateMap.retainedCaravanData.Notify_GeneratedTempIncidentMapFor(caravan);
			}
			return orGenerateMap;
		}

		// Token: 0x04005917 RID: 22807
		private const int MapCellsPerPawn = 900;

		// Token: 0x04005918 RID: 22808
		private const int MinMapSize = 75;

		// Token: 0x04005919 RID: 22809
		private const int MaxMapSize = 110;
	}
}
