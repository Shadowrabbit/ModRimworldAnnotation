using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C6 RID: 1990
	public class JobGiver_Meditate : ThinkNode_JobGiver
	{
		// Token: 0x060035B0 RID: 13744 RVA: 0x0012F7A8 File Offset: 0x0012D9A8
		public override float GetPriority(Pawn pawn)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			bool flag = pawn.HasPsylink && psychicEntropy != null && psychicEntropy.CurrentPsyfocus < Mathf.Min(psychicEntropy.TargetPsyfocus, 0.95f) && psychicEntropy.PsychicSensitivity > float.Epsilon;
			if (!this.ValidatePawnState(pawn))
			{
				return 0f;
			}
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (((timetable != null) ? timetable.CurrentAssignment : null) == TimeAssignmentDefOf.Meditate)
			{
				return 9f;
			}
			if (pawn.CurrentBed() == null)
			{
				Pawn_TimetableTracker timetable2 = pawn.timetable;
				if (((timetable2 != null) ? timetable2.CurrentAssignment : null) == TimeAssignmentDefOf.Anything && flag)
				{
					return 7.1f;
				}
			}
			else if (flag && pawn.health.hediffSet.PainTotal <= 0.3f && pawn.CurrentBed() != null)
			{
				return 6f;
			}
			return 0f;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0012F876 File Offset: 0x0012DA76
		protected virtual bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() == null && !MeditationUtility.CanOnlyMeditateInBed(pawn);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0012F88B File Offset: 0x0012DA8B
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			return MeditationUtility.GetMeditationJob(pawn, false);
		}
	}
}
