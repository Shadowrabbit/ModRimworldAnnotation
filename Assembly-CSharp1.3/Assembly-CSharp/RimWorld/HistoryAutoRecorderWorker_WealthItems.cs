using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B02 RID: 2818
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x06004249 RID: 16969 RVA: 0x00163168 File Offset: 0x00161368
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthItems;
				}
			}
			return num;
		}
	}
}
