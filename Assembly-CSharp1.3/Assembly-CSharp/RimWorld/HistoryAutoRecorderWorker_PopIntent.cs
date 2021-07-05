using System;

namespace RimWorld
{
	// Token: 0x02000B08 RID: 2824
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		// Token: 0x06004255 RID: 16981 RVA: 0x00163296 File Offset: 0x00161496
		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
