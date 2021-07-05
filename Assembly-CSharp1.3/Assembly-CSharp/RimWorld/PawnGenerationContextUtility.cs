using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001001 RID: 4097
	public static class PawnGenerationContextUtility
	{
		// Token: 0x0600607B RID: 24699 RVA: 0x0020DDDC File Offset: 0x0020BFDC
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			switch (context)
			{
			case PawnGenerationContext.All:
				return "PawnGenerationContext_All".Translate();
			case PawnGenerationContext.PlayerStarter:
				return "PawnGenerationContext_PlayerStarter".Translate();
			case PawnGenerationContext.NonPlayer:
				return "PawnGenerationContext_NonPlayer".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0020DE32 File Offset: 0x0020C032
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0020DE40 File Offset: 0x0020C040
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x0020DE74 File Offset: 0x0020C074
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
