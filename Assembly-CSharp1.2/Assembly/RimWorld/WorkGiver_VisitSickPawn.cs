using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DAD RID: 3501
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x00037420 File Offset: 0x00035620
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x000380D1 File Offset: 0x000362D1
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x001B53C4 File Offset: 0x001B35C4
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

		// Token: 0x06004FD3 RID: 20435 RVA: 0x001B5414 File Offset: 0x001B3614
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow);
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x001B5438 File Offset: 0x001B3638
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Job job = JobMaker.MakeJob(JobDefOf.VisitSickPawn, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
