using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001775 RID: 6005
	public static class TileFinder
	{
		// Token: 0x06008A77 RID: 35447 RVA: 0x0031B0D0 File Offset: 0x003192D0
		public static int RandomStartingTile()
		{
			return TileFinder.RandomSettlementTileFor(Faction.OfPlayer, true, null);
		}

		// Token: 0x06008A78 RID: 35448 RVA: 0x0031B0E0 File Offset: 0x003192E0
		public static int RandomSettlementTileFor(Faction faction, bool mustBeAutoChoosable = false, Predicate<int> extraValidator = null)
		{
			Func<int, float> <>9__1;
			for (int i = 0; i < 500; i++)
			{
				IEnumerable<int> source = from _ in Enumerable.Range(0, 100)
				select Rand.Range(0, Find.WorldGrid.TilesCount);
				Func<int, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = delegate(int x)
					{
						Tile tile = Find.WorldGrid[x];
						if (!tile.biome.canBuildBase || !tile.biome.implemented || tile.hilliness == Hilliness.Impassable)
						{
							return 0f;
						}
						if (mustBeAutoChoosable && !tile.biome.canAutoChoose)
						{
							return 0f;
						}
						if (extraValidator != null && !extraValidator(x))
						{
							return 0f;
						}
						float num2 = tile.biome.settlementSelectionWeight;
						Faction faction2 = faction;
						if (((faction2 != null) ? faction2.def.minSettlementTemperatureChanceCurve : null) != null)
						{
							num2 *= faction.def.minSettlementTemperatureChanceCurve.Evaluate(GenTemperature.MinTemperatureAtTile(x));
						}
						return num2;
					});
				}
				int num;
				if (source.TryRandomElementByWeight(weightSelector, out num) && TileFinder.IsValidTileForNewSettlement(num, null))
				{
					return num;
				}
			}
			Log.Error("Failed to find faction base tile for " + faction);
			return 0;
		}

		// Token: 0x06008A79 RID: 35449 RVA: 0x0031B190 File Offset: 0x00319390
		public static bool IsValidTileForNewSettlement(int tile, StringBuilder reason = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (!tile2.biome.canBuildBase)
			{
				if (reason != null)
				{
					reason.Append("CannotLandBiome".Translate(tile2.biome.LabelCap));
				}
				return false;
			}
			if (!tile2.biome.implemented)
			{
				if (reason != null)
				{
					reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.biome.LabelCap);
				}
				return false;
			}
			if (tile2.hilliness == Hilliness.Impassable)
			{
				if (reason != null)
				{
					reason.Append("CannotLandImpassableMountains".Translate());
				}
				return false;
			}
			Settlement settlement = Find.WorldObjects.SettlementBaseAt(tile);
			if (settlement != null)
			{
				if (reason != null)
				{
					if (settlement.Faction == null)
					{
						reason.Append("TileOccupied".Translate());
					}
					else if (settlement.Faction == Faction.OfPlayer)
					{
						reason.Append("YourBaseAlreadyThere".Translate());
					}
					else
					{
						reason.Append("BaseAlreadyThere".Translate(settlement.Faction.Name));
					}
				}
				return false;
			}
			if (Find.WorldObjects.AnySettlementBaseAtOrAdjacent(tile))
			{
				if (reason != null)
				{
					reason.Append("FactionBaseAdjacent".Translate());
				}
				return false;
			}
			if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null || Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedSettlement, tile))
			{
				if (reason != null)
				{
					reason.Append("TileOccupied".Translate());
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008A7A RID: 35450 RVA: 0x0031B338 File Offset: 0x00319538
		public static bool TryFindPassableTileWithTraversalDistance(int rootTile, int minDist, int maxDist, out int result, Predicate<int> validator = null, bool ignoreFirstTilePassability = false, TileFinderMode tileFinderMode = TileFinderMode.Random, bool canTraverseImpassable = false, bool exitOnFirstTileFound = false)
		{
			TileFinder.tmpTiles.Clear();
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => canTraverseImpassable || !Find.World.Impassable(x) || (x == rootTile & ignoreFirstTilePassability), delegate(int tile, int traversalDistance)
			{
				if (traversalDistance > maxDist)
				{
					return true;
				}
				if (traversalDistance >= minDist && !Find.World.Impassable(tile) && (validator == null || validator(tile)))
				{
					TileFinder.tmpTiles.Add(new Pair<int, int>(tile, traversalDistance));
					if (exitOnFirstTileFound)
					{
						return true;
					}
				}
				return false;
			}, int.MaxValue, null);
			if (exitOnFirstTileFound)
			{
				if (TileFinder.tmpTiles.Count > 0)
				{
					result = TileFinder.tmpTiles[0].First;
					return true;
				}
				result = -1;
				return false;
			}
			else
			{
				switch (tileFinderMode)
				{
				case TileFinderMode.Random:
				{
					Pair<int, int> pair;
					if (TileFinder.tmpTiles.TryRandomElement(out pair))
					{
						result = pair.First;
						return true;
					}
					result = -1;
					return false;
				}
				case TileFinderMode.Near:
				{
					Pair<int, int> pair;
					if (TileFinder.tmpTiles.TryRandomElementByWeight((Pair<int, int> x) => 1f - (float)(x.Second - minDist) / ((float)(maxDist - minDist) + 0.01f), out pair))
					{
						result = pair.First;
						return true;
					}
					result = -1;
					return false;
				}
				case TileFinderMode.Furthest:
					if (TileFinder.tmpTiles.Count > 0)
					{
						int maxDistanceWithOffset = Mathf.Clamp(TileFinder.tmpTiles.MaxBy((Pair<int, int> t) => t.Second).Second - 2, minDist, maxDist);
						Pair<int, int> pair2;
						if ((from t in TileFinder.tmpTiles
						where t.Second >= maxDistanceWithOffset - 1
						select t).TryRandomElement(out pair2))
						{
							result = pair2.First;
							return true;
						}
					}
					result = -1;
					return false;
				default:
					Log.Error(string.Format("Unknown tile distance preference {0}.", tileFinderMode));
					result = -1;
					return false;
				}
			}
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x0031B4F0 File Offset: 0x003196F0
		public static bool TryFindRandomPlayerTile(out int tile, bool allowCaravans, Predicate<int> validator = null)
		{
			TileFinder.tmpPlayerTiles.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapPawns.FreeColonistsSpawnedCount != 0 && (validator == null || validator(maps[i].Tile)))
				{
					TileFinder.tmpPlayerTiles.Add(maps[i].Tile);
				}
			}
			if (allowCaravans)
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled && (validator == null || validator(caravans[j].Tile)))
					{
						TileFinder.tmpPlayerTiles.Add(caravans[j].Tile);
					}
				}
			}
			if (TileFinder.tmpPlayerTiles.TryRandomElement(out tile))
			{
				return true;
			}
			Map map;
			if ((from x in Find.Maps
			where x.IsPlayerHome && (validator == null || validator(x.Tile))
			select x).TryRandomElement(out map))
			{
				tile = map.Tile;
				return true;
			}
			Map map2;
			if ((from x in Find.Maps
			where x.mapPawns.FreeColonistsSpawnedCount != 0 && (validator == null || validator(x.Tile))
			select x).TryRandomElement(out map2))
			{
				tile = map2.Tile;
				return true;
			}
			Caravan caravan;
			if (!allowCaravans && (from x in Find.WorldObjects.Caravans
			where x.IsPlayerControlled && (validator == null || validator(x.Tile))
			select x).TryRandomElement(out caravan))
			{
				tile = caravan.Tile;
				return true;
			}
			tile = -1;
			return false;
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x0031B694 File Offset: 0x00319894
		public static bool TryFindNewSiteTile(out int tile, int minDist = 7, int maxDist = 27, bool allowCaravans = false, TileFinderMode tileFinderMode = TileFinderMode.Near, int nearThisTile = -1, bool exitOnFirstTileFound = false)
		{
			Func<int, int> findTile = delegate(int root)
			{
				int result;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist, maxDist, out result, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && TileFinder.IsValidTileForNewSettlement(x, null), false, tileFinderMode, false, exitOnFirstTileFound))
				{
					return result;
				}
				if (TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist, maxDist, out result, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && TileFinder.IsValidTileForNewSettlement(x, null) && (!Find.World.Impassable(x) || Find.WorldGrid[x].WaterCovered), false, tileFinderMode, true, exitOnFirstTileFound))
				{
					return result;
				}
				return -1;
			};
			int arg;
			if (nearThisTile != -1)
			{
				arg = nearThisTile;
			}
			else if (!TileFinder.TryFindRandomPlayerTile(out arg, allowCaravans, (int x) => findTile(x) != -1))
			{
				tile = -1;
				return false;
			}
			tile = findTile(arg);
			return tile != -1;
		}

		// Token: 0x04005827 RID: 22567
		private static List<Pair<int, int>> tmpTiles = new List<Pair<int, int>>();

		// Token: 0x04005828 RID: 22568
		private static List<int> tmpPlayerTiles = new List<int>();
	}
}
