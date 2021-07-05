using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015A2 RID: 5538
	public class SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x060082BB RID: 33467 RVA: 0x002E77AC File Offset: 0x002E59AC
		public static bool CanResolve(string symbol, ResolveParams rp)
		{
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j] is SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal && ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060082BC RID: 33468 RVA: 0x002E7826 File Offset: 0x002E5A26
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms;
		}

		// Token: 0x060082BD RID: 33469 RVA: 0x002E784C File Offset: 0x002E5A4C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("worshippedTerminal", rp, null);
			BaseGen.globalSettings.basePart_worshippedTerminalsResolved++;
		}

		// Token: 0x040051D8 RID: 20952
		private const float MaxCoverage = 0.08f;
	}
}
