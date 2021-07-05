using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5C RID: 2908
	public class QuestPart_VisitColony : QuestPart_MakeLord
	{
		// Token: 0x060043FE RID: 17406 RVA: 0x001698C0 File Offset: 0x00167AC0
		protected override Lord MakeLord()
		{
			IntVec3 chillSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out chillSpot))
			{
				chillSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			LordJob_VisitColony lordJob = new LordJob_VisitColony(this.faction ?? this.pawns[0].Faction, chillSpot, this.durationTicks);
			return LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x00169950 File Offset: 0x00167B50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.durationTicks, "durationTicks", null, false);
		}

		// Token: 0x04002946 RID: 10566
		public int? durationTicks;
	}
}
