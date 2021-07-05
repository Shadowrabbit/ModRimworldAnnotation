using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class JobGiver_GiveSpeech : ThinkNode_JobGiver
	{
		// Token: 0x0600353A RID: 13626 RVA: 0x0012D2CC File Offset: 0x0012B4CC
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Building_Throne building_Throne = duty.focusSecond.Thing as Building_Throne;
			if (building_Throne == null || building_Throne.AssignedPawn != pawn)
			{
				return null;
			}
			if (!pawn.CanReach(building_Throne, PathEndMode.InteractionCell, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, duty.focusSecond);
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			return job;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x0012D358 File Offset: 0x0012B558
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GiveSpeech jobGiver_GiveSpeech = (JobGiver_GiveSpeech)base.DeepCopy(resolve);
			jobGiver_GiveSpeech.soundDefMale = this.soundDefMale;
			jobGiver_GiveSpeech.soundDefFemale = this.soundDefFemale;
			return jobGiver_GiveSpeech;
		}

		// Token: 0x04001E7E RID: 7806
		public SoundDef soundDefMale;

		// Token: 0x04001E7F RID: 7807
		public SoundDef soundDefFemale;
	}
}
