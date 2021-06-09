using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200207D RID: 8317
	public static class GenPlanetMorphology
	{
		// Token: 0x0600B054 RID: 45140 RVA: 0x00333428 File Offset: 0x00331628
		public static void Erode(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			GenPlanetMorphology.tilesSet.Clear();
			GenPlanetMorphology.tilesSet.AddRange(tiles);
			GenPlanetMorphology.tmpEdgeTiles.Clear();
			for (int i = 0; i < tiles.Count; i++)
			{
				worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
				for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
				{
					if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
					{
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
			}
			if (!GenPlanetMorphology.tmpEdgeTiles.Any<int>())
			{
				return;
			}
			GenPlanetMorphology.tmpOutput.Clear();
			Predicate<int> passCheck;
			if (extraPredicate != null)
			{
				passCheck = ((int x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				passCheck = ((int x) => GenPlanetMorphology.tilesSet.Contains(x));
			}
			Find.WorldFloodFiller.FloodFill(-1, passCheck, delegate(int tile, int traversalDist)
			{
				if (traversalDist >= count)
				{
					GenPlanetMorphology.tmpOutput.Add(tile);
				}
				return false;
			}, int.MaxValue, GenPlanetMorphology.tmpEdgeTiles);
			tiles.Clear();
			tiles.AddRange(GenPlanetMorphology.tmpOutput);
		}

		// Token: 0x0600B055 RID: 45141 RVA: 0x00333560 File Offset: 0x00331760
		public static void Dilate(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> passCheck = extraPredicate;
			if (extraPredicate == null && (passCheck = GenPlanetMorphology.<>c.<>9__5_0) == null)
			{
				passCheck = (GenPlanetMorphology.<>c.<>9__5_0 = ((int x) => true));
			}
			worldFloodFiller.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDist)
			{
				if (traversalDist > count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					tiles.Add(tile);
				}
				return false;
			}, int.MaxValue, tiles);
		}

		// Token: 0x0600B056 RID: 45142 RVA: 0x00072A1F File Offset: 0x00070C1F
		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		// Token: 0x0600B057 RID: 45143 RVA: 0x00072A31 File Offset: 0x00070C31
		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}

		// Token: 0x04007957 RID: 31063
		private static HashSet<int> tmpOutput = new HashSet<int>();

		// Token: 0x04007958 RID: 31064
		private static HashSet<int> tilesSet = new HashSet<int>();

		// Token: 0x04007959 RID: 31065
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x0400795A RID: 31066
		private static List<int> tmpEdgeTiles = new List<int>();
	}
}
