using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200167D RID: 5757
	public class QuestNode_GetNearestHomeMapOf : QuestNode
	{
		// Token: 0x060085FF RID: 34303 RVA: 0x00300D6F File Offset: 0x002FEF6F
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x00300D79 File Offset: 0x002FEF79
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06008601 RID: 34305 RVA: 0x00300D88 File Offset: 0x002FEF88
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				Map mapHeld = this.mapOf.GetValue(slate).MapHeld;
				if (mapHeld != null && mapHeld.IsPlayerHome)
				{
					slate.Set<Map>(this.storeAs.GetValue(slate), mapHeld, false);
					return;
				}
				int tile = this.mapOf.GetValue(slate).Tile;
				if (tile != -1)
				{
					Map map = null;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						if (maps[i].IsPlayerHome && (map == null || Find.WorldGrid.ApproxDistanceInTiles(tile, maps[i].Tile) < Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile)))
						{
							map = maps[i];
						}
					}
					if (map != null)
					{
						slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
					}
				}
			}
		}

		// Token: 0x040053C6 RID: 21446
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040053C7 RID: 21447
		public SlateRef<Thing> mapOf;
	}
}
