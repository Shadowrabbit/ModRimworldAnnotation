using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ED5 RID: 3797
	public abstract class ThoughtWorker_MusicalInstrumentListeningBase : ThoughtWorker
	{
		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x0600541B RID: 21531
		protected abstract ThingDef InstrumentDef { get; }

		// Token: 0x0600541C RID: 21532 RVA: 0x001C2BD4 File Offset: 0x001C0DD4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			ThingDef def = this.InstrumentDef;
			return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(def), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), def.building.instrumentRange, delegate(Thing thing)
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				return building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed && Building_MusicalInstrument.IsAffectedByInstrument(def, building_MusicalInstrument.Position, p.Position, p.Map);
			}, null, 0, -1, false, RegionType.Set_Passable, false) != null;
		}
	}
}
