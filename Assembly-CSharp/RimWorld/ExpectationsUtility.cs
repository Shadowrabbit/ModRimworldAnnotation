using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAE RID: 7342
	public static class ExpectationsUtility
	{
		// Token: 0x06009FBA RID: 40890 RVA: 0x002EAC64 File Offset: 0x002E8E64
		public static void Reset()
		{
			ExpectationsUtility.wealthExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.WealthTriggered
			orderby ed.order
			select ed).ToList<ExpectationDef>();
		}

		// Token: 0x06009FBB RID: 40891 RVA: 0x002EACC8 File Offset: 0x002E8EC8
		public static ExpectationDef CurrentExpectationFor(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (p.Faction != Faction.OfPlayer && !p.IsPrisonerOfColony)
			{
				return ExpectationDefOf.ExtremelyLow;
			}
			if (p.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (p.royalty != null && p.MapHeld.IsPlayerHome)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						RoyalTitle currentTitleInFaction = p.royalty.GetCurrentTitleInFaction(faction);
						if (currentTitleInFaction != null && currentTitleInFaction.conceited && currentTitleInFaction.def.minExpectation != null && currentTitleInFaction.def.minExpectation.order > expectationDef.order)
						{
							expectationDef = currentTitleInFaction.def.minExpectation;
						}
					}
				}
				return expectationDef;
			}
			return ExpectationDefOf.VeryLow;
		}

		// Token: 0x06009FBC RID: 40892 RVA: 0x002EADBC File Offset: 0x002E8FBC
		public static ExpectationDef CurrentExpectationFor(Map m)
		{
			float wealthTotal = m.wealthWatcher.WealthTotal;
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (wealthTotal < expectationDef.maxMapWealth)
				{
					return expectationDef;
				}
			}
			return ExpectationsUtility.wealthExpectationsInOrder[ExpectationsUtility.wealthExpectationsInOrder.Count - 1];
		}

		// Token: 0x04006C84 RID: 27780
		private static List<ExpectationDef> wealthExpectationsInOrder;
	}
}
