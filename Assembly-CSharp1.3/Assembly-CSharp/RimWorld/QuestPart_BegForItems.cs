using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B55 RID: 2901
	public class QuestPart_BegForItems : QuestPart_MakeLord
	{
		// Token: 0x060043E1 RID: 17377 RVA: 0x00169088 File Offset: 0x00167288
		protected override Lord MakeLord()
		{
			IntVec3 idleSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out idleSpot))
			{
				idleSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			return LordMaker.MakeNewLord(this.faction, new LordJob_BegForItems(this.faction, idleSpot, this.target, this.thingDef, this.amount, this.outSignalItemsReceived), base.Map, null);
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x001690F7 File Offset: 0x001672F7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x0400292A RID: 10538
		public Pawn target;

		// Token: 0x0400292B RID: 10539
		public ThingDef thingDef;

		// Token: 0x0400292C RID: 10540
		public int amount;

		// Token: 0x0400292D RID: 10541
		public string outSignalItemsReceived;
	}
}
