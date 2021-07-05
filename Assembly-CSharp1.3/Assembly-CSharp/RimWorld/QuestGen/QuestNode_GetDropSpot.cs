using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166D RID: 5741
	public class QuestNode_GetDropSpot : QuestNode
	{
		// Token: 0x060085B8 RID: 34232 RVA: 0x002FF518 File Offset: 0x002FD718
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
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		// Token: 0x060085B9 RID: 34233 RVA: 0x002FF588 File Offset: 0x002FD788
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
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x060085BA RID: 34234 RVA: 0x002FF5FC File Offset: 0x002FD7FC
		private bool TryFindDropSpot(Map map, float minDistFromEdge, out IntVec3 spawnSpot)
		{
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map) && (float)x.DistanceToEdge(map) >= minDistFromEdge && map.reachability.CanReachColony(x), map, 1000, out spawnSpot);
		}

		// Token: 0x04005388 RID: 21384
		[NoTranslate]
		public SlateRef<string> storeAs = "dropSpot";

		// Token: 0x04005389 RID: 21385
		public SlateRef<float> minDistanceFromEdge;
	}
}
