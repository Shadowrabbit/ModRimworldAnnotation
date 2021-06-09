using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F5E RID: 8030
	public class QuestNode_GetSiteTile : QuestNode
	{
		// Token: 0x0600AB47 RID: 43847 RVA: 0x0031EAA4 File Offset: 0x0031CCA4
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

		// Token: 0x0600AB48 RID: 43848 RVA: 0x0031EAD4 File Offset: 0x0031CCD4
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

		// Token: 0x0600AB49 RID: 43849 RVA: 0x0031EB10 File Offset: 0x0031CD10
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
			return TileFinder.TryFindNewSiteTile(out tile, intRange.min, intRange.max, this.allowCaravans.GetValue(slate), this.preferCloserTiles.GetValue(slate), nearThisTile);
		}

		// Token: 0x04007498 RID: 29848
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007499 RID: 29849
		public SlateRef<bool> preferCloserTiles;

		// Token: 0x0400749A RID: 29850
		public SlateRef<bool> allowCaravans;

		// Token: 0x0400749B RID: 29851
		public SlateRef<bool?> clampRangeBySiteParts;

		// Token: 0x0400749C RID: 29852
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
