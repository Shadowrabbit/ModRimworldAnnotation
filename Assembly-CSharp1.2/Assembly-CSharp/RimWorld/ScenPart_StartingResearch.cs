using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001614 RID: 5652
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x06007AE6 RID: 31462 RVA: 0x0024FAEC File Offset: 0x0024DCEC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.project.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<ResearchProjectDef>(this.NonRedundantResearchProjects(), (ResearchProjectDef d) => d.LabelCap, (ResearchProjectDef d) => delegate()
				{
					this.project = d;
				});
			}
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x000529F4 File Offset: 0x00050BF4
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x00052A07 File Offset: 0x00050C07
		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					return true;
				}
				return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
			});
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x00052A32 File Offset: 0x00050C32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x00052A4A File Offset: 0x00050C4A
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x00052A6B File Offset: 0x00050C6B
		public override void PostGameStart()
		{
			Find.ResearchManager.FinishProject(this.project, false, null);
		}

		// Token: 0x04005086 RID: 20614
		private ResearchProjectDef project;
	}
}
