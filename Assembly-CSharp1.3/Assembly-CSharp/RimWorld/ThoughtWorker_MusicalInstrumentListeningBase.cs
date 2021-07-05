using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C9 RID: 2505
	public abstract class ThoughtWorker_MusicalInstrumentListeningBase : ThoughtWorker
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003E2F RID: 15919
		protected abstract ThingDef InstrumentDef { get; }

		// Token: 0x06003E30 RID: 15920 RVA: 0x0015499C File Offset: 0x00152B9C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			ThingDef def = this.InstrumentDef;
			return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(def), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false), def.building.instrumentRange, delegate(Thing thing)
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				return building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed && Building_MusicalInstrument.IsAffectedByInstrument(def, building_MusicalInstrument.Position, p.Position, p.Map);
			}, null, 0, -1, false, RegionType.Set_Passable, false) != null;
		}
	}
}
