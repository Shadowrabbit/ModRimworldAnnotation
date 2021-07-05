using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F26 RID: 7974
	public class QuestNode_GetMap : QuestNode
	{
		// Token: 0x0600AA7F RID: 43647 RVA: 0x0031BCAC File Offset: 0x00319EAC
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

		// Token: 0x0600AA80 RID: 43648 RVA: 0x0031BD00 File Offset: 0x00319F00
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

		// Token: 0x0600AA81 RID: 43649 RVA: 0x0031BD5C File Offset: 0x00319F5C
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

		// Token: 0x0600AA82 RID: 43650 RVA: 0x0031BDFC File Offset: 0x00319FFC
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			IntVec3 intVec;
			return map != null && (!this.mustBeInfestable.GetValue(slate) || InfestationCellFinder.TryFindCell(out intVec, map));
		}

		// Token: 0x040073DD RID: 29661
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040073DE RID: 29662
		public SlateRef<bool> mustBeInfestable;

		// Token: 0x040073DF RID: 29663
		public SlateRef<int> preferMapWithMinFreeColonists;
	}
}
