using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B59 RID: 2905
	public class QuestPart_ExitOnShuttle : QuestPart_MakeLord
	{
		// Token: 0x060043EE RID: 17390 RVA: 0x0016952A File Offset: 0x0016772A
		public QuestPart_ExitOnShuttle()
		{
			ModLister.CheckRoyaltyOrIdeology("Shuttle");
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x00169544 File Offset: 0x00167744
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_ExitOnShuttle(this.shuttle, this.addFleeToil), base.Map, null);
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x00169569 File Offset: 0x00167769
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04002939 RID: 10553
		public Thing shuttle;

		// Token: 0x0400293A RID: 10554
		public bool addFleeToil = true;
	}
}
