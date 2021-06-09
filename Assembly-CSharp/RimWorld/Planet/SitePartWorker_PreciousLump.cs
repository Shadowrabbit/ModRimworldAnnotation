using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x0200216A RID: 8554
	public class SitePartWorker_PreciousLump : SitePartWorker
	{
		// Token: 0x0600B644 RID: 46660 RVA: 0x0007642D File Offset: 0x0007462D
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}

		// Token: 0x0600B645 RID: 46661 RVA: 0x0034B380 File Offset: 0x00349580
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
