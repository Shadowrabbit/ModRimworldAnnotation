using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001012 RID: 4114
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x06006102 RID: 24834 RVA: 0x0020F9C0 File Offset: 0x0020DBC0
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

		// Token: 0x06006103 RID: 24835 RVA: 0x0020FA29 File Offset: 0x0020DC29
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06006104 RID: 24836 RVA: 0x0020FA3C File Offset: 0x0020DC3C
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

		// Token: 0x06006105 RID: 24837 RVA: 0x0020FA67 File Offset: 0x0020DC67
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x0020FA7F File Offset: 0x0020DC7F
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x0020FAA0 File Offset: 0x0020DCA0
		public override void PostGameStart()
		{
			Find.ResearchManager.FinishProject(this.project, false, null);
		}

		// Token: 0x0400375D RID: 14173
		private ResearchProjectDef project;
	}
}
