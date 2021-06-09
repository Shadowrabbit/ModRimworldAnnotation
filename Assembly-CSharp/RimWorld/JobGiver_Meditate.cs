using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD8 RID: 3288
	public class JobGiver_Meditate : ThinkNode_JobGiver
	{
		// Token: 0x06004BDB RID: 19419 RVA: 0x001A7484 File Offset: 0x001A5684
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

		// Token: 0x06004BDC RID: 19420 RVA: 0x00036032 File Offset: 0x00034232
		protected virtual bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() == null && !MeditationUtility.CanOnlyMeditateInBed(pawn);
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x00036047 File Offset: 0x00034247
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
