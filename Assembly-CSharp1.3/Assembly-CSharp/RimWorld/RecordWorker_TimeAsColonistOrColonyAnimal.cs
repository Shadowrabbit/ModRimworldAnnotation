using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD8 RID: 3544
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x06005243 RID: 21059 RVA: 0x001BC386 File Offset: 0x001BA586
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.HasExtraHomeFaction(null);
		}
	}
}
