using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B06 RID: 2822
	public class HistoryAutoRecorderWorker_Adaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06004251 RID: 16977 RVA: 0x00163274 File Offset: 0x00161474
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherAdaptation.AdaptDays;
		}
	}
}
