using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D97 RID: 7575
	public abstract class RoyalTitleAwardWorker
	{
		// Token: 0x0600A47F RID: 42111 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void OnPreAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x0600A480 RID: 42112
		public abstract void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle);

		// Token: 0x04006F86 RID: 28550
		public RoyalTitleDef def;
	}
}
