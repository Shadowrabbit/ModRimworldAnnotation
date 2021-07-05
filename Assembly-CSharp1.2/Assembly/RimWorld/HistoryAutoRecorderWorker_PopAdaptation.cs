using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001034 RID: 4148
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A62 RID: 23138 RVA: 0x0003EA42 File Offset: 0x0003CC42
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
