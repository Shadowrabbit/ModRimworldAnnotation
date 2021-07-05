using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200086A RID: 2154
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x001398A1 File Offset: 0x00137AA1
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0013DEA3 File Offset: 0x0013C0A3
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0013DEBC File Offset: 0x0013C0BC
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			if (!InteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return true;
			}
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0013DF0C File Offset: 0x0013C10C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0013DF30 File Offset: 0x0013C130
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Job job = JobMaker.MakeJob(JobDefOf.VisitSickPawn, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
