using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB4 RID: 3764
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x060053B9 RID: 21433 RVA: 0x001C1940 File Offset: 0x001BFB40
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
