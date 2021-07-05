using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001778 RID: 6008
	public static class GenWorldClosest
	{
		// Token: 0x06008A8F RID: 35471 RVA: 0x0031BAD4 File Offset: 0x00319CD4
		public static bool TryFindClosestTile(int rootTile, Predicate<int> predicate, out int foundTile, int maxTilesToScan = 2147483647, bool canSearchThroughImpassable = true)
		{
			int foundTileLocal = -1;
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => canSearchThroughImpassable || !Find.World.Impassable(x), delegate(int t)
			{
				bool flag = predicate(t);
				if (flag)
				{
					foundTileLocal = t;
				}
				return flag;
			}, maxTilesToScan, null);
			foundTile = foundTileLocal;
			return foundTileLocal >= 0;
		}

		// Token: 0x06008A90 RID: 35472 RVA: 0x0031BB36 File Offset: 0x00319D36
		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
