using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class JobGiver_GiveSpeechFacingTarget : ThinkNode_JobGiver
	{
		// Token: 0x0600353D RID: 13629 RVA: 0x0012D380 File Offset: 0x0012B580
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Pawn_MindState mindState = pawn.mindState;
			Rot4? rot;
			if (mindState == null)
			{
				rot = null;
			}
			else
			{
				PawnDuty duty2 = mindState.duty;
				rot = ((duty2 != null) ? new Rot4?(duty2.overrideFacing) : null);
			}
			Rot4? rot2 = rot;
			IntVec3 c = (rot2 != null && rot2.Value.IsValid) ? (pawn.Position + rot2.Value.FacingCell) : duty.spectateRect.CenterCell;
			Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, pawn.Position, c);
			job.showSpeechBubbles = this.showSpeechBubbles;
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual;
			LordToil_Ritual lordToil_Ritual;
			if ((lordJob_Ritual = (((lord != null) ? lord.LordJob : null) as LordJob_Ritual)) != null && (lordToil_Ritual = (lordJob_Ritual.lord.CurLordToil as LordToil_Ritual)) != null)
			{
				job.interaction = lordToil_Ritual.stage.BehaviorForRole(lordJob_Ritual.RoleFor(pawn, false).id).speakerInteraction;
			}
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			return job;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0012D4C2 File Offset: 0x0012B6C2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GiveSpeechFacingTarget jobGiver_GiveSpeechFacingTarget = (JobGiver_GiveSpeechFacingTarget)base.DeepCopy(resolve);
			jobGiver_GiveSpeechFacingTarget.soundDefMale = this.soundDefMale;
			jobGiver_GiveSpeechFacingTarget.soundDefFemale = this.soundDefFemale;
			jobGiver_GiveSpeechFacingTarget.showSpeechBubbles = this.showSpeechBubbles;
			return jobGiver_GiveSpeechFacingTarget;
		}

		// Token: 0x04001E80 RID: 7808
		public SoundDef soundDefMale;

		// Token: 0x04001E81 RID: 7809
		public SoundDef soundDefFemale;

		// Token: 0x04001E82 RID: 7810
		public bool showSpeechBubbles = true;
	}
}
