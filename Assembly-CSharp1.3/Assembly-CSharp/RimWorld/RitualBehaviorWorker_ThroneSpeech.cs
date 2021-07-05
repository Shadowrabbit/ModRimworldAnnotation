using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F26 RID: 3878
	public class RitualBehaviorWorker_ThroneSpeech : RitualBehaviorWorker
	{
		// Token: 0x06005C46 RID: 23622 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_ThroneSpeech()
		{
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_ThroneSpeech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x001FD757 File Offset: 0x001FB957
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Joinable_Speech(target, organizer, ritual, this.def.stages, assignments, true);
		}
	}
}
