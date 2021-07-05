using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FF1 RID: 4081
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		// Token: 0x06006012 RID: 24594 RVA: 0x0020C5BA File Offset: 0x0020A7BA
		public override bool IsAvailable()
		{
			return base.IsAvailable() && Faction.OfInsects != null;
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x0020C5D0 File Offset: 0x0020A7D0
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int mechanoidsCount = this.GetMechanoidsCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("count", mechanoidsCount.ToString()));
			outExtraDescriptionConstants.Add("count", mechanoidsCount.ToString());
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0020C628 File Offset: 0x0020A828
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetMechanoidsCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0020C67C File Offset: 0x0020A87C
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, FactionDefOf.Mechanoid.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			return sitePartParams;
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0020C6A8 File Offset: 0x0020A8A8
		private int GetMechanoidsCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = Faction.OfMechanoids,
				groupKind = PawnGroupKindDefOf.Combat,
				points = parms.threatPoints,
				seed = new int?(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}
	}
}
