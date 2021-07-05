using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFF RID: 2815
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06004243 RID: 16963 RVA: 0x001630FE File Offset: 0x001612FE
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoLodgers.Count<Pawn>();
		}
	}
}
