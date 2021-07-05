using System;

namespace RimWorld
{
	// Token: 0x02001035 RID: 4149
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A64 RID: 23140 RVA: 0x0003EA53 File Offset: 0x0003CC53
		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
