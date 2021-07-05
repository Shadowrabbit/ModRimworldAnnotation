using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001694 RID: 5780
	public class QuestNode_GetWalkInSpot : QuestNode
	{
		// Token: 0x06008661 RID: 34401 RVA: 0x00303348 File Offset: 0x00301548
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

		// Token: 0x06008662 RID: 34402 RVA: 0x003033AC File Offset: 0x003015AC
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

		// Token: 0x06008663 RID: 34403 RVA: 0x00303414 File Offset: 0x00301614
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

		// Token: 0x04005430 RID: 21552
		[NoTranslate]
		public SlateRef<string> storeAs = "walkInSpot";
	}
}
