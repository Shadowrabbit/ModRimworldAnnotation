using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000827 RID: 2087
	public abstract class WorkGiver_Warden : WorkGiver_Scanner
	{
		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003774 RID: 14196 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003775 RID: 14197 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x00138BC5 File Offset: 0x00136DC5
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x00138BD7 File Offset: 0x00136DD7
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawnedCount == 0;
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x00138BEC File Offset: 0x00136DEC
		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner, bool forced = false)
		{
			Pawn pawn = prisoner as Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !prisoner.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x00138C58 File Offset: 0x00136E58
		protected bool ShouldTakeCareOfSlave(Pawn warden, Thing slave, bool forced = false)
		{
			Pawn pawn = slave as Pawn;
			return pawn != null && pawn.IsSlaveOfColony && pawn.guest.SlaveIsSecure && pawn.Spawned && !pawn.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.Touch, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x00138CBC File Offset: 0x00136EBC
		protected bool IsExecutionIdeoAllowed(Pawn warden, Pawn victim)
		{
			return new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job() && (!victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerGuilty, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerInnocent, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}
	}
}
