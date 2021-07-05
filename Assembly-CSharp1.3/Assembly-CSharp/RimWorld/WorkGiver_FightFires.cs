using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083D RID: 2109
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x0013B0EB File Offset: 0x001392EB
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0013B0F8 File Offset: 0x001392F8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Fire fire = t as Fire;
			if (fire == null)
			{
				return false;
			}
			Pawn pawn2 = fire.parent as Pawn;
			if (pawn2 != null)
			{
				if (pawn2 == pawn)
				{
					return false;
				}
				if ((pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction || pawn2.HostFaction == pawn.HostFaction) && !pawn.Map.areaManager.Home[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
				{
					return false;
				}
				if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Firefighting))
				{
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position])
				{
					JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
					return false;
				}
			}
			return ((pawn.Position - fire.Position).LengthHorizontalSquared <= 225 || pawn.CanReserve(fire, 1, -1, null, forced)) && !WorkGiver_FightFires.FireIsBeingHandled(fire, pawn);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0013B212 File Offset: 0x00139412
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.BeatFire, t);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x0013B224 File Offset: 0x00139424
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			if (!f.Spawned)
			{
				return false;
			}
			Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler);
			return pawn != null && pawn.Position.InHorDistOf(f.Position, 5f);
		}

		// Token: 0x04001F23 RID: 7971
		private const int NearbyPawnRadius = 15;

		// Token: 0x04001F24 RID: 7972
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x04001F25 RID: 7973
		private const float HandledDistance = 5f;
	}
}
