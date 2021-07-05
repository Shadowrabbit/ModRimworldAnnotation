using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139D RID: 5021
	public class PawnColumnWorker_ManhunterOnDamageChance : PawnColumnWorker_Text
	{
		// Token: 0x06007A34 RID: 31284 RVA: 0x002B2018 File Offset: 0x002B0218
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn, null);
			if (manhunterOnDamageChance == 0f)
			{
				return "-";
			}
			return manhunterOnDamageChance.ToStringPercent();
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x002B2041 File Offset: 0x002B0241
		protected override string GetTip(Pawn pawn)
		{
			return "HarmedRevengeChanceExplanation".Translate();
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x002B2054 File Offset: 0x002B0254
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnUtility.GetManhunterOnDamageChance(a, null).CompareTo(PawnUtility.GetManhunterOnDamageChance(b, null));
		}
	}
}
