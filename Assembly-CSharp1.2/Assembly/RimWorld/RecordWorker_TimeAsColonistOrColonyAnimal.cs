using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144F RID: 5199
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x06007063 RID: 28771 RVA: 0x0004BC9E File Offset: 0x00049E9E
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.HasExtraHomeFaction(null);
		}
	}
}
