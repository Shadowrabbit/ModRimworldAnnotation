using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000055 RID: 85
	public class WorldFloodFiller
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00014F94 File Offset: 0x00013194
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00014FC8 File Offset: 0x000131C8
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014FFC File Offset: 0x000131FC
		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (int tile, int traversalDistance) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00015030 File Offset: 0x00013230
		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.working = true;
			this.ClearVisited();
			if (rootTile != -1 && extraRootTiles == null && !passCheck(rootTile))
			{
				this.working = false;
				return;
			}
			int tilesCount = Find.WorldGrid.TilesCount;
			int num = tilesCount;
			if (this.traversalDistance.Count != tilesCount)
			{
				this.traversalDistance.Clear();
				for (int i = 0; i < tilesCount; i++)
				{
					this.traversalDistance.Add(-1);
				}
			}
			WorldGrid worldGrid = Find.WorldGrid;
			List<int> tileIDToNeighbors_offsets = worldGrid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = worldGrid.tileIDToNeighbors_values;
			int num2 = 0;
			this.openSet.Clear();
			if (rootTile != -1)
			{
				this.visited.Add(rootTile);
				this.traversalDistance[rootTile] = 0;
				this.openSet.Enqueue(rootTile);
			}
			if (extraRootTiles == null)
			{
				goto IL_25F;
			}
			this.visited.AddRange(extraRootTiles);
			IList<int> list = extraRootTiles as IList<int>;
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					int num3 = list[j];
					this.traversalDistance[num3] = 0;
					this.openSet.Enqueue(num3);
				}
				goto IL_25F;
			}
			using (IEnumerator<int> enumerator = extraRootTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num4 = enumerator.Current;
					this.traversalDistance[num4] = 0;
					this.openSet.Enqueue(num4);
				}
				goto IL_25F;
			}
			IL_16E:
			int num5 = this.openSet.Dequeue();
			int num6 = this.traversalDistance[num5];
			if (processor(num5, num6))
			{
				goto IL_270;
			}
			num2++;
			if (num2 == maxTilesToProcess)
			{
				goto IL_270;
			}
			int num7 = (num5 + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[num5 + 1] : tileIDToNeighbors_values.Count;
			for (int k = tileIDToNeighbors_offsets[num5]; k < num7; k++)
			{
				int num8 = tileIDToNeighbors_values[k];
				if (this.traversalDistance[num8] == -1 && passCheck(num8))
				{
					this.visited.Add(num8);
					this.openSet.Enqueue(num8);
					this.traversalDistance[num8] = num6 + 1;
				}
			}
			if (this.openSet.Count > num)
			{
				Log.Error("Overflow on world flood fill (>" + num + " cells). Make sure we're not flooding over the same area after we check it.");
				this.working = false;
				return;
			}
			IL_25F:
			if (this.openSet.Count > 0)
			{
				goto IL_16E;
			}
			IL_270:
			this.working = false;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000152C4 File Offset: 0x000134C4
		private void ClearVisited()
		{
			int i = 0;
			int count = this.visited.Count;
			while (i < count)
			{
				this.traversalDistance[this.visited[i]] = -1;
				i++;
			}
			this.visited.Clear();
			this.openSet.Clear();
		}

		// Token: 0x04000126 RID: 294
		private bool working;

		// Token: 0x04000127 RID: 295
		private Queue<int> openSet = new Queue<int>();

		// Token: 0x04000128 RID: 296
		private List<int> traversalDistance = new List<int>();

		// Token: 0x04000129 RID: 297
		private List<int> visited = new List<int>();
	}
}
