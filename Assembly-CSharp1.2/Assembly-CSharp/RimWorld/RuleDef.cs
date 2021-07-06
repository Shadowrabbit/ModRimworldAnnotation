using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD8 RID: 4056
	public class RuleDef : Def
	{
		// Token: 0x04003AAD RID: 15021
		[NoTranslate]
		public string symbol;

		// Token: 0x04003AAE RID: 15022
		public List<SymbolResolver> resolvers;
	}
}
