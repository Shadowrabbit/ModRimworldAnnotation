using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001532 RID: 5426
	public static class RoyalTitleDefExt
	{
		// Token: 0x06008112 RID: 33042 RVA: 0x002DA774 File Offset: 0x002D8974
		public static RoyalTitleDef GetNextTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num == -1 && currentTitle != null)
			{
				return null;
			}
			int num2 = (currentTitle == null) ? 0 : (num + 1);
			if (faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count <= num2)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num2];
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x002DA7CC File Offset: 0x002D89CC
		public static RoyalTitleDef GetPreviousTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num];
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x002DA81C File Offset: 0x002D8A1C
		public static RoyalTitleDef GetPreviousTitle_IncludeNonRewardable(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAllInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAllInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAllInSeniorityOrderForReading[num];
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x002DA86C File Offset: 0x002D8A6C
		public static bool TryInherit(this RoyalTitleDef title, Pawn from, Faction faction, out RoyalTitleInheritanceOutcome outcome)
		{
			outcome = default(RoyalTitleInheritanceOutcome);
			if (title.GetInheritanceWorker(faction) == null)
			{
				return false;
			}
			Pawn heir = from.royalty.GetHeir(faction);
			if (heir == null || heir.Destroyed)
			{
				return false;
			}
			RoyalTitleDef currentTitle = heir.royalty.GetCurrentTitle(faction);
			bool heirTitleHigher = currentTitle != null && currentTitle.seniority >= title.seniority;
			outcome = new RoyalTitleInheritanceOutcome
			{
				heir = heir,
				heirCurrentTitle = currentTitle,
				heirTitleHigher = heirTitleHigher
			};
			return true;
		}
	}
}
