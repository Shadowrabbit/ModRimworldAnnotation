using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B00 RID: 2816
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06004245 RID: 16965 RVA: 0x0016310B File Offset: 0x0016130B
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
