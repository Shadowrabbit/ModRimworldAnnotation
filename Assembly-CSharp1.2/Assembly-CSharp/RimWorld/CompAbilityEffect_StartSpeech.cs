using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001390 RID: 5008
	public class CompAbilityEffect_StartSpeech : CompAbilityEffect
	{
		// Token: 0x06006CA8 RID: 27816 RVA: 0x00049E7B File Offset: 0x0004807B
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			this.parent.pawn.drafter.Drafted = false;
			this.parent.pawn.Map.lordsStarter.TryStartReigningSpeech(this.parent.pawn);
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x00215D78 File Offset: 0x00213F78
		public override bool GizmoDisabled(out string reason)
		{
			Lord lord = this.parent.pawn.GetLord();
			LordJob_Joinable_Speech lordJob_Joinable_Speech = ((lord != null) ? lord.LordJob : null) as LordJob_Joinable_Speech;
			if (lordJob_Joinable_Speech != null && lordJob_Joinable_Speech.Organizer == this.parent.pawn)
			{
				reason = "AbilitySpeechDisabledAlreadyGivingSpeech".Translate();
				return true;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(this.parent.pawn.Map))
			{
				reason = "AbilitySpeechDisabledAnotherGatheringInProgress".Translate();
				return true;
			}
			Building_Throne assignedThrone = this.parent.pawn.ownership.AssignedThrone;
			if (assignedThrone == null)
			{
				reason = "AbilitySpeechDisabledNoThroneAssigned".Translate();
				return true;
			}
			if (!this.parent.pawn.CanReserveAndReach(assignedThrone, PathEndMode.InteractionCell, this.parent.pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				reason = "AbilitySpeechDisabledNoThroneIsNotAccessible".Translate();
				return true;
			}
			if (this.parent.pawn.royalty.GetUnmetThroneroomRequirements(true, false).Any<string>())
			{
				reason = "AbilitySpeechDisabledNoThroneUndignified".Translate();
				return true;
			}
			reason = null;
			return false;
		}
	}
}
