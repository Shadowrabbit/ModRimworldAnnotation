using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000891 RID: 2193
	public class LordJob_Ritual_Mutilation : LordJob_Ritual
	{
		// Token: 0x06003A0C RID: 14860 RVA: 0x00145A97 File Offset: 0x00143C97
		public LordJob_Ritual_Mutilation()
		{
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x00145AAA File Offset: 0x00143CAA
		public LordJob_Ritual_Mutilation(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer)
		{
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x00145AC6 File Offset: 0x00143CC6
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return base.ShouldCallOffBecausePawnNoLongerOwned(p) && !this.mutilatedPawns.Contains(p);
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00145AE2 File Offset: 0x00143CE2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.mutilatedPawns, "mutilatedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04001FEA RID: 8170
		public List<Pawn> mutilatedPawns = new List<Pawn>();
	}
}
