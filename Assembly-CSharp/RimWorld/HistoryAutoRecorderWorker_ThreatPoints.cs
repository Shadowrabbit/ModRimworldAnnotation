using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001032 RID: 4146
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A5E RID: 23134 RVA: 0x0003EA12 File Offset: 0x0003CC12
		public override float PullRecord()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return 0f;
			}
			return StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap) / 10f;
		}
	}
}
