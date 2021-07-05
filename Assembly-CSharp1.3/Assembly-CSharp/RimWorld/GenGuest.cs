using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E31 RID: 3633
	public static class GenGuest
	{
		// Token: 0x06005409 RID: 21513 RVA: 0x001C6EC0 File Offset: 0x001C50C0
		public static void PrisonerRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasImprisoned, null, null);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x001C6F18 File Offset: 0x001C5118
		public static void SlaveRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasEnslaved, null, null);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x001C6F70 File Offset: 0x001C5170
		private static void GuestRelease(Pawn p)
		{
			if (p.ownership != null)
			{
				p.ownership.UnclaimAll();
			}
			if (((!p.IsSlave || p.SlaveFaction == Faction.OfPlayer) && p.Faction == Faction.OfPlayer) || p.IsWildMan())
			{
				p.guest.SetGuestStatus(null, GuestStatus.Guest);
				if (p.IsWildMan())
				{
					p.mindState.WildManEverReachedOutside = false;
					return;
				}
			}
			else
			{
				p.guest.Released = true;
				IntVec3 c;
				if (RCellFinder.TryFindBestExitSpot(p, out c, TraverseMode.ByPawn))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
					job.exitMapOnArrival = true;
					p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
				}
			}
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x001C7028 File Offset: 0x001C5228
		public static void AddHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			if (!prisoner.IsColonist)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
				{
					if (pawn.needs.mood != null && pawn != prisoner)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner, null);
					}
				}
			}
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x001C70B0 File Offset: 0x001C52B0
		public static void RemoveHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists)
			{
				if (pawn.needs.mood != null && pawn != prisoner)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner);
				}
			}
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x001C712C File Offset: 0x001C532C
		public static void EmancipateSlave(Pawn warden, Pawn slave)
		{
			if (!slave.IsSlave)
			{
				return;
			}
			GenGuest.SlaveRelease(slave);
			if (slave.IsWildMan())
			{
				slave.mindState.WildManEverReachedOutside = false;
			}
			Messages.Message("MessageSlaveEmancipated".Translate(slave, warden), new LookTargets(new TargetInfo[]
			{
				slave,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x001C71A8 File Offset: 0x001C53A8
		public static void EnslavePrisoner(Pawn warden, Pawn prisoner)
		{
			if (prisoner.IsSlave)
			{
				return;
			}
			bool everEnslaved = prisoner.guest.EverEnslaved;
			prisoner.guest.SetGuestStatus(warden.Faction, GuestStatus.Slave);
			Messages.Message("MessagePrisonerEnslaved".Translate(prisoner, warden), new LookTargets(new TargetInfo[]
			{
				prisoner,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, warden.Named(HistoryEventArgsNames.Doer)), true);
			if (!everEnslaved)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisonerNotPreviouslyEnslaved, warden.Named(HistoryEventArgsNames.Doer)), true);
			}
		}
	}
}
