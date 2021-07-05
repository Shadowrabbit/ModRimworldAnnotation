using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001523 RID: 5411
	public class RoyalTitleAwardWorker
	{
		// Token: 0x060080B2 RID: 32946 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnPreAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x060080B3 RID: 32947 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x0400502A RID: 20522
		public RoyalTitleDef def;
	}
}
