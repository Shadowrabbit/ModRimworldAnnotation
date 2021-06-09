using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102C RID: 4140
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06005A52 RID: 23122 RVA: 0x0003E9F8 File Offset: 0x0003CBF8
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers.Count<Pawn>();
		}
	}
}
