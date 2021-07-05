using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001674 RID: 5748
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		// Token: 0x060085D6 RID: 34262 RVA: 0x002FFC90 File Offset: 0x002FDE90
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		// Token: 0x060085D7 RID: 34263 RVA: 0x002FFCCC File Offset: 0x002FDECC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x002FFCFC File Offset: 0x002FDEFC
		private int GetLargestSize(Slate slate)
		{
			Map mapResolved = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			if (mapResolved == null)
			{
				return 0;
			}
			int value = this.max.GetValue(slate);
			CellRect cellRect = LargestAreaFinder.FindLargestRect(mapResolved, (IntVec3 x) => this.IsClear(x, mapResolved), value);
			return Mathf.Min(new int[]
			{
				cellRect.Width,
				cellRect.Height,
				value
			});
		}

		// Token: 0x060085D9 RID: 34265 RVA: 0x002FFD8C File Offset: 0x002FDF8C
		private bool IsClear(IntVec3 c, Map map)
		{
			if (!c.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial && thingList[i].Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (thingList[i].def.mineable)
				{
					bool flag = false;
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[j];
						if (c2.InBounds(map) && c2.GetFirstMineable(map) == null)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040053A7 RID: 21415
		public SlateRef<Map> map;

		// Token: 0x040053A8 RID: 21416
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053A9 RID: 21417
		public SlateRef<int> failIfSmaller;

		// Token: 0x040053AA RID: 21418
		public SlateRef<int> max;
	}
}
