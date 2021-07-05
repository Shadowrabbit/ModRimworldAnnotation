using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170C RID: 5900
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		// Token: 0x06008844 RID: 34884 RVA: 0x0030F9A4 File Offset: 0x0030DBA4
		private bool TryFindRootTile(out int tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(int x)
			{
				int num;
				return this.TryFindDestinationTileActual(x, 180, out num);
			});
		}

		// Token: 0x06008845 RID: 34885 RVA: 0x0030F9BC File Offset: 0x0030DBBC
		private bool TryFindDestinationTile(int rootTile, out int tile)
		{
			int num = 800;
			for (int i = 0; i < 1000; i++)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 180)
				{
					num = 180;
				}
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					return true;
				}
				if (num <= 180)
				{
					return false;
				}
			}
			tile = -1;
			return false;
		}

		// Token: 0x06008846 RID: 34886 RVA: 0x0030FA1C File Offset: 0x0030DC1C
		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			for (int i = 0; i < 2; i++)
			{
				bool canTraverseImpassable = i == 1;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose, true, TileFinderMode.Near, canTraverseImpassable, false))
				{
					return true;
				}
			}
			tile = -1;
			return false;
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x0030FA74 File Offset: 0x0030DC74
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int rootTile;
			this.TryFindRootTile(out rootTile);
			int var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x06008848 RID: 34888 RVA: 0x0030FAB0 File Offset: 0x0030DCB0
		protected override bool TestRunInt(Slate slate)
		{
			int rootTile;
			int num;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out num);
		}

		// Token: 0x0400562C RID: 22060
		private const int MinTraversalDistance = 180;

		// Token: 0x0400562D RID: 22061
		private const int MaxTraversalDistance = 800;

		// Token: 0x0400562E RID: 22062
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
