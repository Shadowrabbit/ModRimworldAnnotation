using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FDE RID: 8158
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		// Token: 0x0600AD10 RID: 44304 RVA: 0x00070CA1 File Offset: 0x0006EEA1
		private bool TryFindRootTile(out int tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(int x)
			{
				int num;
				return this.TryFindDestinationTileActual(x, 180, out num);
			});
		}

		// Token: 0x0600AD11 RID: 44305 RVA: 0x0032656C File Offset: 0x0032476C
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

		// Token: 0x0600AD12 RID: 44306 RVA: 0x003265CC File Offset: 0x003247CC
		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			for (int i = 0; i < 2; i++)
			{
				bool canTraverseImpassable = i == 1;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose, true, true, canTraverseImpassable))
				{
					return true;
				}
			}
			tile = -1;
			return false;
		}

		// Token: 0x0600AD13 RID: 44307 RVA: 0x00326624 File Offset: 0x00324824
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int rootTile;
			this.TryFindRootTile(out rootTile);
			int var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600AD14 RID: 44308 RVA: 0x00326660 File Offset: 0x00324860
		protected override bool TestRunInt(Slate slate)
		{
			int rootTile;
			int num;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out num);
		}

		// Token: 0x04007694 RID: 30356
		private const int MinTraversalDistance = 180;

		// Token: 0x04007695 RID: 30357
		private const int MaxTraversalDistance = 800;

		// Token: 0x04007696 RID: 30358
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
