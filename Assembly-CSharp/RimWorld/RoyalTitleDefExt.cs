using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBF RID: 7615
	public static class RoyalTitleDefExt
	{
		// Token: 0x0600A590 RID: 42384 RVA: 0x00300DA0 File Offset: 0x002FEFA0
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

		// Token: 0x0600A591 RID: 42385 RVA: 0x00300DF8 File Offset: 0x002FEFF8
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

		// Token: 0x0600A592 RID: 42386 RVA: 0x00300E48 File Offset: 0x002FF048
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

		// Token: 0x0600A593 RID: 42387 RVA: 0x00300E98 File Offset: 0x002FF098
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
