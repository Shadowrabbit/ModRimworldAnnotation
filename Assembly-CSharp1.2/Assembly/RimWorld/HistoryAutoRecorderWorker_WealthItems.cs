using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102F RID: 4143
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A58 RID: 23128 RVA: 0x001D50AC File Offset: 0x001D32AC
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
