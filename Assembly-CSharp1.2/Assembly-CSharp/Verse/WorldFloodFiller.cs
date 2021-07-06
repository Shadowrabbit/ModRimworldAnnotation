using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000099 RID: 153
	public class WorldFloodFiller
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x0008AFFC File Offset: 0x000891FC
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0008B030 File Offset: 0x00089230
		public void FloodFill(int rootTile, Predicate<int> passCheck, Action<int, int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, delegate(int tile, int traversalDistance)
			{
				processor(tile, traversalDistance);
				return false;
			}, maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0008B064 File Offset: 0x00089264
		public void FloodFill(int rootTile, Predicate<int> passCheck, Predicate<int> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			this.FloodFill(rootTile, passCheck, (int tile, int traversalDistance) => processor(tile), maxTilesToProcess, extraRootTiles);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0008B098 File Offset: 0x00089298
		public void FloodFill(int rootTile, Predicate<int> passCheck, Func<int, int, bool> processor, int maxTilesToProcess = 2147483647, IEnumerable<int> extraRootTiles = null)
		{
			if (this.working)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
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
				goto IL_261;
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
				goto IL_261;
			}
			using (IEnumerator<int> enumerator = extraRootTiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num4 = enumerator.Current;
					this.traversalDistance[num4] = 0;
					this.openSet.Enqueue(num4);
				}
				goto IL_261;
			}
			IL_16F:
			int num5 = this.openSet.Dequeue();
			int num6 = this.traversalDistance[num5];
			if (processor(num5, num6))
			{
				goto IL_272;
			}
			num2++;
			if (num2 == maxTilesToProcess)
			{
				goto IL_272;
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
				Log.Error("Overflow on world flood fill (>" + num + " cells). Make sure we're not flooding over the same area after we check it.", false);
				this.working = false;
				return;
			}
			IL_261:
			if (this.openSet.Count > 0)
			{
				goto IL_16F;
			}
			IL_272:
			this.working = false;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0008B330 File Offset: 0x00089530
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

		// Token: 0x0400028E RID: 654
		private bool working;

		// Token: 0x0400028F RID: 655
		private Queue<int> openSet = new Queue<int>();

		// Token: 0x04000290 RID: 656
		private List<int> traversalDistance = new List<int>();

		// Token: 0x04000291 RID: 657
		private List<int> visited = new List<int>();
	}
}
