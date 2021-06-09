using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102D RID: 4141
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A54 RID: 23124 RVA: 0x0003EA05 File Offset: 0x0003CC05
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
