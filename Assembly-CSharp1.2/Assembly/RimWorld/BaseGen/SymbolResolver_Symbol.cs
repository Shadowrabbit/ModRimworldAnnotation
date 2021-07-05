using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E6E RID: 7790
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x0600A7E9 RID: 42985 RVA: 0x0030E114 File Offset: 0x0030C314
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != this.symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A7EA RID: 42986 RVA: 0x0006EC21 File Offset: 0x0006CE21
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp, null);
		}

		// Token: 0x040071FA RID: 29178
		public string symbol;
	}
}
