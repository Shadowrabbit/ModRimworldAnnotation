using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F24 RID: 7972
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		// Token: 0x0600AA78 RID: 43640 RVA: 0x0031BAF0 File Offset: 0x00319CF0
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		// Token: 0x0600AA79 RID: 43641 RVA: 0x0031BB2C File Offset: 0x00319D2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		// Token: 0x0600AA7A RID: 43642 RVA: 0x0031BB5C File Offset: 0x00319D5C
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

		// Token: 0x0600AA7B RID: 43643 RVA: 0x0031BBEC File Offset: 0x00319DEC
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

		// Token: 0x040073D7 RID: 29655
		public SlateRef<Map> map;

		// Token: 0x040073D8 RID: 29656
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073D9 RID: 29657
		public SlateRef<int> failIfSmaller;

		// Token: 0x040073DA RID: 29658
		public SlateRef<int> max;
	}
}
