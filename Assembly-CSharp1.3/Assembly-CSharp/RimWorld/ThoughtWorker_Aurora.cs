using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x06003DE2 RID: 15842 RVA: 0x00153A70 File Offset: 0x00151C70
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
