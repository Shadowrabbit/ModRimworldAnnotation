using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x020017DD RID: 6109
	public class SitePartWorker_PreciousLump : SitePartWorker
	{
		// Token: 0x06008E40 RID: 36416 RVA: 0x0020BDC6 File Offset: 0x00209FC6
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}

		// Token: 0x06008E41 RID: 36417 RVA: 0x003318C0 File Offset: 0x0032FAC0
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (part.site.ActualThreatPoints > 0f)
			{
				outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", "\n\n" + "PreciousLumpHostileThreat".Translate()));
				return;
			}
			outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", ""));
		}
	}
}
