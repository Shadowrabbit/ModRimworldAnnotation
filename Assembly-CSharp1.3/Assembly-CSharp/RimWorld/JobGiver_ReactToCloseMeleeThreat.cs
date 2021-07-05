using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public class JobGiver_ReactToCloseMeleeThreat : ThinkNode_JobGiver
	{
		// Token: 0x060035CE RID: 13774 RVA: 0x00130CD0 File Offset: 0x0012EED0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn meleeThreat = pawn.mindState.meleeThreat;
			if (meleeThreat == null)
			{
				return null;
			}
			if (meleeThreat.IsInvisible())
			{
				return null;
			}
			if (this.IsHunting(pawn, meleeThreat))
			{
				return null;
			}
			if (this.IsDueling(pawn, meleeThreat))
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				return null;
			}
			if (!pawn.mindState.MeleeThreatStillThreat)
			{
				pawn.mindState.meleeThreat = null;
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, meleeThreat);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = 200;
			job.reactingToMeleeThreat = true;
			return job;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00130D90 File Offset: 0x0012EF90
		private bool IsDueling(Pawn pawn, Pawn other)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Duel lordJob_Ritual_Duel;
			return (lordJob_Ritual_Duel = (((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel)) != null && lordJob_Ritual_Duel.Opponent(pawn) == other;
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00130DC4 File Offset: 0x0012EFC4
		private bool IsHunting(Pawn pawn, Pawn prey)
		{
			if (pawn.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn.jobs.curDriver as JobDriver_Hunt;
			if (jobDriver_Hunt != null)
			{
				return jobDriver_Hunt.Victim == prey;
			}
			JobDriver_PredatorHunt jobDriver_PredatorHunt = pawn.jobs.curDriver as JobDriver_PredatorHunt;
			return jobDriver_PredatorHunt != null && jobDriver_PredatorHunt.Prey == prey;
		}

		// Token: 0x04001EC1 RID: 7873
		private const int MaxMeleeChaseTicks = 200;
	}
}
