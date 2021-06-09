using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F34 RID: 7988
	public class QuestNode_GetNearestHomeMapOf : QuestNode
	{
		// Token: 0x0600AAB4 RID: 43700 RVA: 0x0006FCC4 File Offset: 0x0006DEC4
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AAB5 RID: 43701 RVA: 0x0006FCCE File Offset: 0x0006DECE
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AAB6 RID: 43702 RVA: 0x0031CB88 File Offset: 0x0031AD88
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

		// Token: 0x04007406 RID: 29702
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04007407 RID: 29703
		public SlateRef<Thing> mapOf;
	}
}
