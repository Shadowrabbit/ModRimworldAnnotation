using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102E RID: 4142
	public class HistoryAutoRecorderWorker_WealthBuildings : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A56 RID: 23126 RVA: 0x001D505C File Offset: 0x001D325C
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthBuildings;
				}
			}
			return num;
		}
	}
}
