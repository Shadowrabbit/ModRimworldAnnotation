using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA9 RID: 2985
	public class QuestPart_SetSitePartThreatPointsToCurrent : QuestPart
	{
		// Token: 0x060045A5 RID: 17829 RVA: 0x00170E84 File Offset: 0x0016F084
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.useMapParentThreatPoints != null && this.useMapParentThreatPoints.HasMap && this.site != null && this.sitePartDef != null)
			{
				List<SitePart> parts = this.site.parts;
				for (int i = 0; i < parts.Count; i++)
				{
					if (parts[i].def == this.sitePartDef)
					{
						parts[i].parms.threatPoints = StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map) * this.threatPointsFactor;
					}
				}
			}
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x00170F2C File Offset: 0x0016F12C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<SitePartDef>(ref this.sitePartDef, "sitePartDef");
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			Scribe_Values.Look<float>(ref this.threatPointsFactor, "threatPointsFactor", 0f, false);
		}

		// Token: 0x04002A64 RID: 10852
		public string inSignal;

		// Token: 0x04002A65 RID: 10853
		public SitePartDef sitePartDef;

		// Token: 0x04002A66 RID: 10854
		public Site site;

		// Token: 0x04002A67 RID: 10855
		public MapParent useMapParentThreatPoints;

		// Token: 0x04002A68 RID: 10856
		public float threatPointsFactor = 1f;
	}
}
