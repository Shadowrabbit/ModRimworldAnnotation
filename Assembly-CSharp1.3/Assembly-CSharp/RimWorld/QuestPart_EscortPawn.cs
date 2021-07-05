using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B58 RID: 2904
	public class QuestPart_EscortPawn : QuestPart_MakeLord
	{
		// Token: 0x060043EC RID: 17388 RVA: 0x001694DB File Offset: 0x001676DB
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_EscortPawn(this.escortee, this.shuttle), base.Map, null);
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x00169500 File Offset: 0x00167700
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x04002937 RID: 10551
		public Pawn escortee;

		// Token: 0x04002938 RID: 10552
		public Thing shuttle;
	}
}
