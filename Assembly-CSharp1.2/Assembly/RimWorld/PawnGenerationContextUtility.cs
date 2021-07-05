using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E7 RID: 5607
	public static class PawnGenerationContextUtility
	{
		// Token: 0x060079D5 RID: 31189 RVA: 0x0024DC64 File Offset: 0x0024BE64
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

		// Token: 0x060079D6 RID: 31190 RVA: 0x00051FFC File Offset: 0x000501FC
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x060079D7 RID: 31191 RVA: 0x0024DCBC File Offset: 0x0024BEBC
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x00052007 File Offset: 0x00050207
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
