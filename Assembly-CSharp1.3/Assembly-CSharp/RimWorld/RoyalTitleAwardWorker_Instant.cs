using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001524 RID: 5412
	public class RoyalTitleAwardWorker_Instant : RoyalTitleAwardWorker
	{
		// Token: 0x060080B5 RID: 32949 RVA: 0x002D9877 File Offset: 0x002D7A77
		public override void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			pawn.royalty.TryUpdateTitle(faction, true, this.def);
		}
	}
}
