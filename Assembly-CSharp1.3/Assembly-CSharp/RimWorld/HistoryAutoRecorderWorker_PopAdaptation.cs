using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B07 RID: 2823
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06004253 RID: 16979 RVA: 0x00163285 File Offset: 0x00161485
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
