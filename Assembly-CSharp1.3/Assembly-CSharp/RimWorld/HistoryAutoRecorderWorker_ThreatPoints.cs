using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B05 RID: 2821
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		// Token: 0x0600424F RID: 16975 RVA: 0x00163255 File Offset: 0x00161455
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
