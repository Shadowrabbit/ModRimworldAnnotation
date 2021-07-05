using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017CD RID: 6093
	public static class SettlementNameGenerator
	{
		// Token: 0x06008D9A RID: 36250 RVA: 0x0032EA50 File Offset: 0x0032CC50
		public static string GenerateSettlementName(Settlement factionBase, RulePackDef rulePack = null)
		{
			if (rulePack == null)
			{
				if (factionBase.Faction == null || factionBase.Faction.def.settlementNameMaker == null)
				{
					return factionBase.def.label;
				}
				rulePack = factionBase.Faction.def.settlementNameMaker;
			}
			SettlementNameGenerator.usedNames.Clear();
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Name != null)
				{
					SettlementNameGenerator.usedNames.Add(settlement.Name);
				}
			}
			return NameGenerator.GenerateName(rulePack, SettlementNameGenerator.usedNames, true, null);
		}

		// Token: 0x0400599A RID: 22938
		private static List<string> usedNames = new List<string>();
	}
}
