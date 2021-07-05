using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001480 RID: 5248
	public static class ExpectationsUtility
	{
		// Token: 0x06007D7E RID: 32126 RVA: 0x002C543C File Offset: 0x002C363C
		public static void Reset()
		{
			ExpectationsUtility.wealthExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.WealthTriggered
			orderby ed.order
			select ed).ToList<ExpectationDef>();
			ExpectationsUtility.roleExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.forRoles
			orderby ed.order
			select ed).ToList<ExpectationDef>();
		}

		// Token: 0x06007D7F RID: 32127 RVA: 0x002C54F8 File Offset: 0x002C36F8
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
				if (ModsConfig.IdeologyActive)
				{
					Ideo ideo = p.Ideo;
					Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
					if (precept_Role != null && precept_Role.def.expectationsOffset != 0)
					{
						ExpectationDef expectationDef2 = ExpectationsUtility.ExpectationForOrder(Math.Max(expectationDef.order + precept_Role.def.expectationsOffset, 0), true);
						if (expectationDef2 != null)
						{
							expectationDef = expectationDef2;
						}
					}
				}
				return expectationDef;
			}
			return ExpectationDefOf.VeryLow;
		}

		// Token: 0x06007D80 RID: 32128 RVA: 0x002C5644 File Offset: 0x002C3844
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

		// Token: 0x06007D81 RID: 32129 RVA: 0x002C56A0 File Offset: 0x002C38A0
		public static ExpectationDef ExpectationForOrder(int order, bool forRole = false)
		{
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (order == expectationDef.order)
				{
					return expectationDef;
				}
			}
			if (forRole)
			{
				for (int j = 0; j < ExpectationsUtility.roleExpectationsInOrder.Count; j++)
				{
					ExpectationDef expectationDef2 = ExpectationsUtility.roleExpectationsInOrder[j];
					if (order == expectationDef2.order)
					{
						return expectationDef2;
					}
				}
			}
			return null;
		}

		// Token: 0x06007D82 RID: 32130 RVA: 0x002C570C File Offset: 0x002C390C
		public static bool OffsetByRole(Pawn p)
		{
			if (ModsConfig.IdeologyActive && p.ideo != null)
			{
				Precept_Role role = p.Ideo.GetRole(p);
				if (role != null && role.def.expectationsOffset != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004E49 RID: 20041
		private static List<ExpectationDef> wealthExpectationsInOrder;

		// Token: 0x04004E4A RID: 20042
		private static List<ExpectationDef> roleExpectationsInOrder;
	}
}
