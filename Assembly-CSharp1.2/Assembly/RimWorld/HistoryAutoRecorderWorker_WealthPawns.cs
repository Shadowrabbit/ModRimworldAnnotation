using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001030 RID: 4144
	public class HistoryAutoRecorderWorker_WealthPawns : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A5A RID: 23130 RVA: 0x001D50FC File Offset: 0x001D32FC
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthPawns;
				}
			}
			return num;
		}
	}
}
