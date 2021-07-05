using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015CA RID: 5578
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x06008352 RID: 33618 RVA: 0x002ECBA0 File Offset: 0x002EADA0
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

		// Token: 0x06008353 RID: 33619 RVA: 0x002ECC17 File Offset: 0x002EAE17
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp, null);
		}

		// Token: 0x04005203 RID: 20995
		public string symbol;
	}
}
