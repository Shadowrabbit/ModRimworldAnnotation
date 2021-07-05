using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139E RID: 5022
	public class PawnColumnWorker_ManhunterOnTameFailChance : PawnColumnWorker_Text
	{
		// Token: 0x06007A38 RID: 31288 RVA: 0x002B2078 File Offset: 0x002B0278
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnTameFailChance = pawn.RaceProps.manhunterOnTameFailChance;
			if (manhunterOnTameFailChance == 0f)
			{
				return "-";
			}
			return manhunterOnTameFailChance.ToStringPercent();
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x002B20A5 File Offset: 0x002B02A5
		protected override string GetTip(Pawn pawn)
		{
			return "Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate();
		}

		// Token: 0x06007A3A RID: 31290 RVA: 0x002B20B6 File Offset: 0x002B02B6
		public override int Compare(Pawn a, Pawn b)
		{
			return a.RaceProps.manhunterOnTameFailChance.CompareTo(b.RaceProps.manhunterOnTameFailChance);
		}
	}
}
