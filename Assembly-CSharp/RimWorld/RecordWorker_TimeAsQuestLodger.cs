using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001450 RID: 5200
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		// Token: 0x06007065 RID: 28773 RVA: 0x0004BCC1 File Offset: 0x00049EC1
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
