using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FF RID: 4351
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005F12 RID: 24338 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x001E1AC4 File Offset: 0x001DFCC4
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

		// Token: 0x06005F14 RID: 24340 RVA: 0x001E1B48 File Offset: 0x001DFD48
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x00041C83 File Offset: 0x0003FE83
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005F16 RID: 24342 RVA: 0x00041CAC File Offset: 0x0003FEAC
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.minimumTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x04003FA3 RID: 16291
		public RoyalTitleDef minimumTitle;

		// Token: 0x04003FA4 RID: 16292
		public Faction faction;
	}
}
