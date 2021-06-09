using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001031 RID: 4145
	public class HistoryAutoRecorderWorker_WealthTotal : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A5C RID: 23132 RVA: 0x001D514C File Offset: 0x001D334C
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthTotal;
				}
			}
			return num;
		}
	}
}
