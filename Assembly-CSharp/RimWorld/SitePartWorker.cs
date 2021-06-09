using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE0 RID: 4064
	public class SitePartWorker
	{
		// Token: 0x060058A2 RID: 22690 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x0003D974 File Offset: 0x0003BB74
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x0003D9A3 File Offset: 0x0003BBA3
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x0003D9CD File Offset: 0x0003BBCD
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x0003D9FA File Offset: 0x0003BBFA
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDestroy(SitePart sitePart)
		{
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		// Token: 0x04003AF0 RID: 15088
		public SitePartDef def;
	}
}
