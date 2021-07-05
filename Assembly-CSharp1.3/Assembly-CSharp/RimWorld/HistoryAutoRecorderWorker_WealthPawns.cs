using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B03 RID: 2819
	public class HistoryAutoRecorderWorker_WealthPawns : HistoryAutoRecorderWorker
	{
		// Token: 0x0600424B RID: 16971 RVA: 0x001631B8 File Offset: 0x001613B8
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
