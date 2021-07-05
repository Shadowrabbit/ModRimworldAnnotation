using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154A RID: 5450
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x06007609 RID: 30217 RVA: 0x00006B8B File Offset: 0x00004D8B
		public DirectPawnRelation()
		{
		}

		// Token: 0x0600760A RID: 30218 RVA: 0x0004FA43 File Offset: 0x0004DC43
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x0004FA60 File Offset: 0x0004DC60
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x04004DF8 RID: 19960
		public PawnRelationDef def;

		// Token: 0x04004DF9 RID: 19961
		public Pawn otherPawn;

		// Token: 0x04004DFA RID: 19962
		public int startTicks;
	}
}
