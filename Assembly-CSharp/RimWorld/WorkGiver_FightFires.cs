using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D77 RID: 3447
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x00037724 File Offset: 0x00035924
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x001B286C File Offset: 0x001B0A6C
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
				if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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

		// Token: 0x06004EAA RID: 20138 RVA: 0x00037730 File Offset: 0x00035930
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.BeatFire, t);
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x001B2988 File Offset: 0x001B0B88
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			if (!f.Spawned)
			{
				return false;
			}
			Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler);
			return pawn != null && pawn.Position.InHorDistOf(f.Position, 5f);
		}

		// Token: 0x04003342 RID: 13122
		private const int NearbyPawnRadius = 15;

		// Token: 0x04003343 RID: 13123
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x04003344 RID: 13124
		private const float HandledDistance = 5f;
	}
}
