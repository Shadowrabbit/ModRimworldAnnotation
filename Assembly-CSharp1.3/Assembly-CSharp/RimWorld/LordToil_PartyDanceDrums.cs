using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008CB RID: 2251
	public class LordToil_PartyDanceDrums : LordToil_Ritual
	{
		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003B42 RID: 15170 RVA: 0x0014AF90 File Offset: 0x00149190
		public new LordToilData_PartyDanceDrums Data
		{
			get
			{
				return (LordToilData_PartyDanceDrums)this.data;
			}
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0014AF9D File Offset: 0x0014919D
		public LordToil_PartyDanceDrums(IntVec3 spot, LordJob_Ritual ritual, RitualStage stage, Pawn organizer) : base(spot, ritual, stage, organizer)
		{
			this.data = new LordToilData_PartyDanceDrums();
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0014AFB8 File Offset: 0x001491B8
		public override void UpdateAllDuties()
		{
			this.reservedThings.Clear();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = this.DutyForPawn(pawn);
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0014B020 File Offset: 0x00149220
		private PawnDuty DutyForPawn(Pawn pawn)
		{
			DutyDef def = this.stage.defaultDuty;
			IntVec3 spot = this.spot;
			LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;
			int num = this.ritual.assignments.Participants.IndexOf(pawn);
			if (num != -1 && (num % 2 == 0 || this.ritual.assignments.Participants.Count == 0))
			{
				Building_MusicalInstrument building_MusicalInstrument = (from m in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>()
				where GatheringsUtility.InGatheringArea(m.InteractionCell, this.spot, pawn.Map) && GatheringWorker_Concert.InstrumentAccessible(m, pawn)
				select m).RandomElementWithFallback(null);
				if (building_MusicalInstrument != null && building_MusicalInstrument.Spawned)
				{
					def = DutyDefOf.PlayTargetInstrument;
					focusSecond = building_MusicalInstrument;
					this.ritual.usedThings.Add(building_MusicalInstrument);
					this.reservedThings.Add(building_MusicalInstrument);
					this.Data.playedInstruments.SetOrAdd(pawn, building_MusicalInstrument);
				}
			}
			return new PawnDuty(def, spot, focusSecond, (LocalTargetInfo)this.ritual.selectedTarget, -1f);
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0014B148 File Offset: 0x00149348
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			if (b.def == ThingDefOf.Drum && this.Data.playedInstruments.ContainsValue(b))
			{
				Pawn key = this.Data.playedInstruments.First((KeyValuePair<Pawn, Building> kv) => kv.Value == b).Key;
				key.mindState.duty = this.DutyForPawn(key);
				key.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}
	}
}
