using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088D RID: 2189
	public class RitualStagePawnSecondFocus : IExposable
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x001435F3 File Offset: 0x001417F3
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stageIndex, "stageIndex", 0, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
		}

		// Token: 0x04001FB4 RID: 8116
		public int stageIndex;

		// Token: 0x04001FB5 RID: 8117
		public Pawn pawn;

		// Token: 0x04001FB6 RID: 8118
		public TargetInfo target;
	}
}
