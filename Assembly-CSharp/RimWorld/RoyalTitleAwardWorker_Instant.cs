using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D99 RID: 7577
	public class RoyalTitleAwardWorker_Instant : RoyalTitleAwardWorker
	{
		// Token: 0x0600A485 RID: 42117 RVA: 0x0006D125 File Offset: 0x0006B325
		public override void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			pawn.royalty.TryUpdateTitle_NewTemp(faction, true, this.def);
		}
	}
}
