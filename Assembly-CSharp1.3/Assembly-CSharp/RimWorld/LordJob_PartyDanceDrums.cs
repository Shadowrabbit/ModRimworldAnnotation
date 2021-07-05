using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	public class LordJob_PartyDanceDrums : LordJob_Ritual
	{
		// Token: 0x060039BF RID: 14783 RVA: 0x001435B7 File Offset: 0x001417B7
		public LordJob_PartyDanceDrums()
		{
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x001435BF File Offset: 0x001417BF
		public LordJob_PartyDanceDrums(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer)
		{
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x001435D0 File Offset: 0x001417D0
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (!ModLister.CheckIdeology("Drum party toil"))
			{
				return null;
			}
			return new LordToil_PartyDanceDrums(this.spot, this, stage, this.organizer);
		}
	}
}
