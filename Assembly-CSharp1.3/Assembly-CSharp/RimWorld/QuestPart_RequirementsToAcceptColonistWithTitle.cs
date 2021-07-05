using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B99 RID: 2969
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x001702BC File Offset: 0x0016E4BC
		public override AcceptanceReport CanAccept()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
			{
				if (this.CanPawnAccept(p))
				{
					return true;
				}
			}
			return new AcceptanceReport("QuestNoColonistWithTitle".Translate(this.minimumTitle.GetLabelCapForBothGenders()));
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x00170340 File Offset: 0x0016E540
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x001703A4 File Offset: 0x0016E5A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004566 RID: 17766 RVA: 0x001703CD File Offset: 0x0016E5CD
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.minimumTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x04002A48 RID: 10824
		public RoyalTitleDef minimumTitle;

		// Token: 0x04002A49 RID: 10825
		public Faction faction;
	}
}
