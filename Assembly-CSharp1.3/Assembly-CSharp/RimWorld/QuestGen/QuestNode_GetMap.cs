using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001675 RID: 5749
	public class QuestNode_GetMap : QuestNode
	{
		// Token: 0x060085DB RID: 34267 RVA: 0x002FFE4C File Offset: 0x002FE04C
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return true;
			}
			if (this.TryFindMap(slate, out map))
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
				return true;
			}
			return false;
		}

		// Token: 0x060085DC RID: 34268 RVA: 0x002FFEA0 File Offset: 0x002FE0A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map;
			if (QuestGen.slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return;
			}
			if (this.TryFindMap(slate, out map))
			{
				QuestGen.slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
			}
		}

		// Token: 0x060085DD RID: 34269 RVA: 0x002FFEFC File Offset: 0x002FE0FC
		private bool TryFindMap(Slate slate, out Map map)
		{
			int minCount;
			if (!this.preferMapWithMinFreeColonists.TryGetValue(slate, out minCount))
			{
				minCount = 1;
			}
			IEnumerable<Map> source = from x in Find.Maps
			where x.IsPlayerHome && this.IsAcceptableMap(x, slate)
			select x;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= minCount
			select x).TryRandomElement(out map))
			{
				return (from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out map);
			}
			return true;
		}

		// Token: 0x060085DE RID: 34270 RVA: 0x002FFF9C File Offset: 0x002FE19C
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			IntVec3 intVec;
			return map != null && (!this.mustBeInfestable.GetValue(slate) || InfestationCellFinder.TryFindCell(out intVec, map));
		}

		// Token: 0x040053AB RID: 21419
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040053AC RID: 21420
		public SlateRef<bool> mustBeInfestable;

		// Token: 0x040053AD RID: 21421
		public SlateRef<int> preferMapWithMinFreeColonists;
	}
}
