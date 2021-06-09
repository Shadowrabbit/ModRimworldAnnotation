using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8F RID: 3983
	public abstract class FeatureWorker
	{
		// Token: 0x06005765 RID: 22373
		public abstract void GenerateWhereAppropriate();

		// Token: 0x06005766 RID: 22374 RVA: 0x001CD46C File Offset: 0x001CB66C
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

		// Token: 0x06005767 RID: 22375 RVA: 0x001CD528 File Offset: 0x001CB728
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

		// Token: 0x06005768 RID: 22376 RVA: 0x0003C9CF File Offset: 0x0003ABCF
		protected static void ClearVisited()
		{
			FeatureWorker.ClearOrCreate<bool>(ref FeatureWorker.visited);
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0003C9DB File Offset: 0x0003ABDB
		protected static void ClearGroupSizes()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupSize);
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0003C9E7 File Offset: 0x0003ABE7
		protected static void ClearGroupIDs()
		{
			FeatureWorker.ClearOrCreate<int>(ref FeatureWorker.groupID);
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x001CD734 File Offset: 0x001CB934
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

		// Token: 0x0400391F RID: 14623
		public FeatureDef def;

		// Token: 0x04003920 RID: 14624
		protected static bool[] visited;

		// Token: 0x04003921 RID: 14625
		protected static int[] groupSize;

		// Token: 0x04003922 RID: 14626
		protected static int[] groupID;

		// Token: 0x04003923 RID: 14627
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04003924 RID: 14628
		private static HashSet<int> tmpTilesForTextDrawPosCalculationSet = new HashSet<int>();

		// Token: 0x04003925 RID: 14629
		private static List<int> tmpEdgeTiles = new List<int>();

		// Token: 0x04003926 RID: 14630
		private static List<Pair<int, int>> tmpTraversedTiles = new List<Pair<int, int>>();
	}
}
