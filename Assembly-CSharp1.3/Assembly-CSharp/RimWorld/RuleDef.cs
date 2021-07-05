using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB8 RID: 2744
	public class RuleDef : Def
	{
		// Token: 0x0400266A RID: 9834
		[NoTranslate]
		public string symbol;

		// Token: 0x0400266B RID: 9835
		public List<SymbolResolver> resolvers;
	}
}
