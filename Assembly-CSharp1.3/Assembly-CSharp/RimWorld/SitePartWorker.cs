using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000ABF RID: 2751
	public class SitePartWorker
	{
		// Token: 0x06004115 RID: 16661 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x0015EC38 File Offset: 0x0015CE38
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0015EC67 File Offset: 0x0015CE67
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = new LookTargets(map.Parent);
			return this.def.arrivedLetter;
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x0015EC8E File Offset: 0x0015CE8E
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0015EC9B File Offset: 0x0015CE9B
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x0015ECC8 File Offset: 0x0015CEC8
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsAvailable()
		{
			return true;
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDestroy(SitePart sitePart)
		{
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		// Token: 0x040026A8 RID: 9896
		public SitePartDef def;
	}
}
