using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001033 RID: 4147
	public class HistoryAutoRecorderWorker_Adaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A60 RID: 23136 RVA: 0x0003EA31 File Offset: 0x0003CC31
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherAdaptation.AdaptDays;
		}
	}
}
