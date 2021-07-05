using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A67 RID: 2663
	public abstract class FeatureWorker
	{
		// Token: 0x06003FF5 RID: 16373
		public abstract void GenerateWhereAppropriate();

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0015AB20 File Offset: 0x00158D20
		protected void AddFeature(List<int> members, List<int> tilesForTextDrawPosCalculation)
		{
			WorldFeature worldFeature = new WorldFeature();
			worldFeature.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
			worldFeature.def = this.def;
			worldFeature.name = NameGenerator.GenerateName(this.def.nameMaker, from x in Find.WorldFeatures.features
			select x.name, false, "r_name");
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < members.Count; i++)
			{
				worldGrid[members[i]].feature = worldFeature;
			}
			this.AssignBestDrawPos(worldFeature, tilesForTextDrawPosCalculation);
			Find.WorldFeatures.features.Add(worldFeature);
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0015ABDC File Offset: 0x00158DDC
		private void AssignBestDrawPos(WorldFeature newFeature, List<int> tilesForTextDrawPosCalculation)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			FeatureWorker.tmpEdgeTiles.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Clear();
			FeatureWorker.tmpTilesForTextDrawPosCalculationSet.AddRange(tilesForTextDrawPosCalculation);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < tilesForTextDrawPosCalculation.Count; i++)
			{
				int num = tilesForTextDrawPosCalculation[i];
				vector += worldGrid.GetTileCenter(num);
				bool flag = worldGrid.IsOnEdge(num);
				if (!flag)
				{
					worldGrid.GetTileNeighbors(num, FeatureWorker.tmpNeighbors);
					for (int j = 0; j < FeatureWorker.tmpNeighbors.Count; j++)
					{
						if (!FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(FeatureWorker.tmpNeighbors[j]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					FeatureWorker.tmpEdgeTiles.Add(num);
				}
			}
			vector /= (float)tilesForTextDrawPosCalculation.Count;
			if (!FeatureWorker.tmpEdgeTiles.Any<int>())
			{
				FeatureWorker.tmpEdgeTiles.Add(tilesForTextDrawPosCalculation.RandomElement<int>());
			}
			int bestTileDist = 0;
			FeatureWorker.tmpTraversedTiles.Clear();
			Find.WorldFloodFiller.FloodFill(-1, (int x) => FeatureWorker.tmpTilesForTextDrawPosCalculationSet.Contains(x), delegate(int tile, int traversalDist)
			{
				FeatureWorker.tmpTraversedTiles.Add(new Pair<int, int>(tile, traversalDist));
				bestTileDist = traversalDist;
				return false;
			}, int.MaxValue, FeatureWorker.tmpEdgeTiles);
			int num2 = -1;
			float num3 = -1f;
			for (int k = 0; k < FeatureWorker.tmpTraversedTiles.Count; k++)
			{
				if (FeatureWorker.tmpTraversedTiles[k].Second == bestTileDist)
				{
					float sqrMagnitude = (worldGrid.GetTileCenter(FeatureWorker.tmpTraversedTiles[k].First) - vector).sqrMagnitude;
					if (num2 == -1 || sqrMagnitude < num3)
					{
						num2 = FeatureWorker.tmpTraversedTiles[k].First;
						num3 = sqrMagnitude;
					}
				}
			}
			float maxDrawSizeInTiles = (float)bestTileDist * 2f * 1.2f;
			newFeature.drawCenter = worldGrid.GetTileCenter(num2);
			newFeature.maxDrawSizeInTiles = maxDrawSizeInTiles;
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0015ADE6 File Offset: 0x00158FE6
		protected static void ClearVisited()
		{
			FeatureWorker.ClearOrCreate<bool>(ref FeatureWorker.visited);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0015ADF2 File Offset: 0x00158FF2
		protected static void ClearGroupSizes()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupSize);
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0015ADFE File Offset: 0x00158FFE
		protected static void ClearGroupIDs()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupID);
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x0015AE0C File Offset: 0x0015900C
		private static void ClearOrCreate<T>(ref T[] array)
		{
			int tilesCount = Find.WorldGrid.TilesCount;
			if (array == null || array.Length != tilesCount)
			{
				array = new T[tilesCount];
				return;
			}
			Array.Clear(array, 0, array.Length);
		}

		// Token: 0x04002436 RID: 9270
		public FeatureDef def;

		// Token: 0x04002437 RID: 9271
		protected static bool[] visited;

		// Token: 0x04002438 RID: 9272
		protected static int[] groupSize;

		// Token: 0x04002439 RID: 9273
		protected static int[] groupID;

		// Token: 0x0400243A RID: 9274
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x0400243B RID: 9275
		private static HashSet<int> tmpTilesForTextDrawPosCalculationSet = new HashSet<int>();

		// Token: 0x0400243C RID: 9276
		private static List<int> tmpEdgeTiles = new List<int>();

		// Token: 0x0400243D RID: 9277
		private static List<Pair<int, int>> tmpTraversedTiles = new List<Pair<int, int>>();
	}
}
