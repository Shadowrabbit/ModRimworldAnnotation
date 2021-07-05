using System;
using RimWorld.Planet;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020012F9 RID: 4857
	public static class NamePlayerSettlementDialogUtility
	{
		// Token: 0x060074B3 RID: 29875 RVA: 0x0027A629 File Offset: 0x00278829
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x0027A64C File Offset: 0x0027884C
		public static void Named(Settlement factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
