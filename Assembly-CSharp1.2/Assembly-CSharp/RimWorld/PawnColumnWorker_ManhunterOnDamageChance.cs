using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B95 RID: 7061
	public class PawnColumnWorker_ManhunterOnDamageChance : PawnColumnWorker_Text
	{
		// Token: 0x06009B96 RID: 39830 RVA: 0x0006781C File Offset: 0x00065A1C
		protected override string GetTextFor(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnDamageChance(pawn, null).ToStringPercent();
		}

		// Token: 0x06009B97 RID: 39831 RVA: 0x0006782A File Offset: 0x00065A2A
		protected override string GetTip(Pawn pawn)
		{
			return "HarmedRevengeChanceExplanation".Translate();
		}
	}
}
