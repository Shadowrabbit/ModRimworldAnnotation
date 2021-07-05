using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F23 RID: 3875
	public class RitualBehaviorWorker_Mutilation : RitualBehaviorWorker
	{
		// Token: 0x06005C35 RID: 23605 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_Mutilation()
		{
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_Mutilation(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x001FD363 File Offset: 0x001FB563
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Mutilation(target, ritual, obligation, this.def.stages, assignments, organizer);
		}
	}
}
