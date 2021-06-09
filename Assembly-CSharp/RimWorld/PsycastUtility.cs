using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013AC RID: 5036
	public class PsycastUtility
	{
		// Token: 0x06006D3E RID: 27966 RVA: 0x00217CD8 File Offset: 0x00215ED8
		public static float TotalEntropyFromQueuedPsycasts(Pawn pawn)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			object obj;
			if (jobs == null)
			{
				obj = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				obj = ((curJob != null) ? curJob.verbToUse : null);
			}
			Verb_CastPsycast verb_CastPsycast = obj as Verb_CastPsycast;
			float num = (verb_CastPsycast != null) ? verb_CastPsycast.Psycast.def.EntropyGain : 0f;
			if (pawn.jobs != null)
			{
				for (int i = 0; i < pawn.jobs.jobQueue.Count; i++)
				{
					Verb_CastPsycast verb_CastPsycast2;
					if ((verb_CastPsycast2 = (pawn.jobs.jobQueue[i].job.verbToUse as Verb_CastPsycast)) != null)
					{
						num += verb_CastPsycast2.Psycast.def.EntropyGain;
					}
				}
			}
			return num;
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x00217D80 File Offset: 0x00215F80
		public static float TotalPsyfocusCostOfQueuedPsycasts(Pawn pawn)
		{
			Pawn_JobTracker jobs = pawn.jobs;
			object obj;
			if (jobs == null)
			{
				obj = null;
			}
			else
			{
				Job curJob = jobs.curJob;
				obj = ((curJob != null) ? curJob.verbToUse : null);
			}
			Verb_CastPsycast verb_CastPsycast = obj as Verb_CastPsycast;
			float num = (verb_CastPsycast != null) ? verb_CastPsycast.Psycast.FinalPsyfocusCost(pawn.jobs.curJob.targetA) : 0f;
			if (pawn.jobs != null)
			{
				for (int i = 0; i < pawn.jobs.jobQueue.Count; i++)
				{
					QueuedJob queuedJob = pawn.jobs.jobQueue[i];
					Verb_CastPsycast verb_CastPsycast2;
					if ((verb_CastPsycast2 = (queuedJob.job.verbToUse as Verb_CastPsycast)) != null)
					{
						num += verb_CastPsycast2.Psycast.FinalPsyfocusCost(queuedJob.job.targetA);
					}
				}
			}
			return num;
		}
	}
}
