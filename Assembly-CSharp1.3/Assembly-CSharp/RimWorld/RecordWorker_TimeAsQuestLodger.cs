using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD9 RID: 3545
	public class RecordWorker_TimeAsQuestLodger : RecordWorker
	{
		// Token: 0x06005245 RID: 21061 RVA: 0x001BC3A9 File Offset: 0x001BA5A9
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.HasExtraHomeFaction(null);
		}
	}
}
