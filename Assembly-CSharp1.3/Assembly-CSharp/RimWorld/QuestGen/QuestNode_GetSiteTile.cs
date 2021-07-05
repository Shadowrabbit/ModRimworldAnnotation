using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001692 RID: 5778
	public class QuestNode_GetSiteTile : QuestNode
	{
		// Token: 0x06008657 RID: 34391 RVA: 0x00302AFC File Offset: 0x00300CFC
		protected override bool TestRunInt(Slate slate)
		{
			int var;
			if (!this.TryFindTile(slate, out var))
			{
				return false;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x06008658 RID: 34392 RVA: 0x00302B2C File Offset: 0x00300D2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int var;
			if (!this.TryFindTile(QuestGen.slate, out var))
			{
				return;
			}
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x06008659 RID: 34393 RVA: 0x00302B68 File Offset: 0x00300D68
		private bool TryFindTile(Slate slate, out int tile)
		{
			Map map = slate.Get<Map>("map", null, false) ?? Find.RandomPlayerHomeMap;
			int nearThisTile = (map != null) ? map.Tile : -1;
			int num = int.MaxValue;
			bool? value = this.clampRangeBySiteParts.GetValue(slate);
			if (value != null && value.Value)
			{
				foreach (SitePartDef sitePartDef in this.sitePartDefs.GetValue(slate))
				{
					if (sitePartDef.conditionCauserDef != null)
					{
						num = Mathf.Min(num, sitePartDef.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
					}
				}
			}
			IntRange intRange;
			if (!slate.TryGet<IntRange>("siteDistRange", out intRange, false))
			{
				intRange = new IntRange(7, Mathf.Min(27, num));
			}
			else if (num != 2147483647)
			{
				intRange = new IntRange(Mathf.Min(intRange.min, num), Mathf.Min(intRange.max, num));
			}
			TileFinderMode tileFinderMode = this.preferCloserTiles.GetValue(slate) ? TileFinderMode.Near : TileFinderMode.Random;
			return TileFinder.TryFindNewSiteTile(out tile, intRange.min, intRange.max, this.allowCaravans.GetValue(slate), tileFinderMode, nearThisTile, false);
		}

		// Token: 0x0400541F RID: 21535
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005420 RID: 21536
		public SlateRef<bool> preferCloserTiles;

		// Token: 0x04005421 RID: 21537
		public SlateRef<bool> allowCaravans;

		// Token: 0x04005422 RID: 21538
		public SlateRef<bool?> clampRangeBySiteParts;

		// Token: 0x04005423 RID: 21539
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
