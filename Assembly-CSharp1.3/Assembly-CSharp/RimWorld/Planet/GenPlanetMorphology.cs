using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001777 RID: 6007
	public static class GenPlanetMorphology
	{
		// Token: 0x06008A8A RID: 35466 RVA: 0x0031B8DC File Offset: 0x00319ADC
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

		// Token: 0x06008A8B RID: 35467 RVA: 0x0031BA14 File Offset: 0x00319C14
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

		// Token: 0x06008A8C RID: 35468 RVA: 0x0031BA85 File Offset: 0x00319C85
		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		// Token: 0x06008A8D RID: 35469 RVA: 0x0031BA97 File Offset: 0x00319C97
		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}

		// Token: 0x0400582B RID: 22571
		private static HashSet<int> tmpOutput = new HashSet<int>();

		// Token: 0x0400582C RID: 22572
		private static HashSet<int> tilesSet = new HashSet<int>();

		// Token: 0x0400582D RID: 22573
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x0400582E RID: 22574
		private static List<int> tmpEdgeTiles = new List<int>();
	}
}
