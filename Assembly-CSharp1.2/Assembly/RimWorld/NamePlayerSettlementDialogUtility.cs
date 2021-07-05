using System;
using RimWorld.Planet;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001A09 RID: 6665
	public static class NamePlayerSettlementDialogUtility
	{
		// Token: 0x0600935B RID: 37723 RVA: 0x00062B7B File Offset: 0x00060D7B
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600935C RID: 37724 RVA: 0x00062B9E File Offset: 0x00060D9E
		public static void Named(Settlement factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
