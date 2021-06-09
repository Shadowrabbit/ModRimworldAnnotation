using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200108E RID: 4238
	public class QuestPart_EscortPawn : QuestPart_MakeLord
	{
		// Token: 0x06005C53 RID: 23635 RVA: 0x000400BF File Offset: 0x0003E2BF
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_EscortPawn(this.escortee, this.shuttle), base.Map, null);
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x000400E4 File Offset: 0x0003E2E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x04003DD6 RID: 15830
		public Pawn escortee;

		// Token: 0x04003DD7 RID: 15831
		public Thing shuttle;
	}
}
