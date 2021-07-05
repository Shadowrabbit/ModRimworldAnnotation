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
	// Token: 0x02000FEE RID: 4078
	public class SitePartWorker_Outpost : SitePartWorker
	{
		// Token: 0x06006005 RID: 24581 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x0020C1D4 File Offset: 0x0020A3D4
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x0020C234 File Offset: 0x0020A434
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x0020C288 File Offset: 0x0020A488
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			sitePartParams.lootMarketValue = SitePartWorker_Outpost.ThreatPointsLootMarketValue.Evaluate(sitePartParams.threatPoints);
			return sitePartParams;
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x0020C2D8 File Offset: 0x0020A4D8
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = site.Faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = parms.threatPoints,
				inhabitants = true,
				seed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x0020C33C File Offset: 0x0020A53C
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x04003723 RID: 14115
		public static readonly SimpleCurve ThreatPointsLootMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 1000f),
				true
			},
			{
				new CurvePoint(10000f, 2000f),
				true
			}
		};
	}
}
