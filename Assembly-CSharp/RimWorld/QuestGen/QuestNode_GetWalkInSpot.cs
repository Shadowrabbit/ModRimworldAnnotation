using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F65 RID: 8037
	public class QuestNode_GetWalkInSpot : QuestNode
	{
		// Token: 0x0600AB5B RID: 43867 RVA: 0x0031F2B8 File Offset: 0x0031D4B8
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return true;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 var;
			if (this.TryFindWalkInSpot(map, out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600AB5C RID: 43868 RVA: 0x0031F31C File Offset: 0x0031D51C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return;
			}
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			IntVec3 var;
			if (this.TryFindWalkInSpot(map, out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x0600AB5D RID: 43869 RVA: 0x0031F384 File Offset: 0x0031D584
		private bool TryFindWalkInSpot(Map map, out IntVec3 spawnSpot)
		{
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map) && map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => true, map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot);
		}

		// Token: 0x040074B1 RID: 29873
		[NoTranslate]
		public SlateRef<string> storeAs = "walkInSpot";
	}
}
