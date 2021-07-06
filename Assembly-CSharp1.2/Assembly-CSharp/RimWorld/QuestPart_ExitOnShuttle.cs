using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200108F RID: 4239
	public class QuestPart_ExitOnShuttle : QuestPart_MakeLord
	{
		// Token: 0x06005C55 RID: 23637 RVA: 0x0004010E File Offset: 0x0003E30E
		public QuestPart_ExitOnShuttle()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttles are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657212, false);
				return;
			}
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x00040135 File Offset: 0x0003E335
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_ExitOnShuttle(this.shuttle, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x0004015A File Offset: 0x0003E35A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04003DD8 RID: 15832
		public Thing shuttle;

		// Token: 0x04003DD9 RID: 15833
		public bool addFleeToil = true;
	}
}
