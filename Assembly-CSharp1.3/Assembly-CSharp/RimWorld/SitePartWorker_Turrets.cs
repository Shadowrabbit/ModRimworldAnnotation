using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FF3 RID: 4083
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x0600601A RID: 24602 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x0020C704 File Offset: 0x0020A904
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
			return sitePartParams;
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x0020C750 File Offset: 0x0020A950
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string threatsInfo = this.GetThreatsInfo(part.parms, part.site.Faction);
			outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0020C791 File Offset: 0x0020A991
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo(sitePart.parms, site.Faction);
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0020C7B8 File Offset: 0x0020A9B8
		private string GetThreatsInfo(SitePartParams parms, Faction faction)
		{
			this.threatsTmp.Clear();
			int num = parms.mortarsCount + 1;
			if (parms.turretsCount != 0)
			{
				string value;
				if (parms.turretsCount == 1)
				{
					value = "Turret".Translate();
				}
				else
				{
					value = "Turrets".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.turretsCount.ToString(), value));
			}
			if (parms.mortarsCount != 0)
			{
				string value;
				if (parms.mortarsCount == 1)
				{
					value = "Mortar".Translate();
				}
				else
				{
					value = "Mortars".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.mortarsCount.ToString(), value));
			}
			if (num != 0)
			{
				string value;
				if (faction == null)
				{
					value = ((num == 1) ? "Enemy".Translate() : "Enemies".Translate());
				}
				else
				{
					value = ((num == 1) ? faction.def.pawnSingular : faction.def.pawnsPlural);
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(num.ToString(), value));
			}
			return this.threatsTmp.ToCommaList(true, false);
		}

		// Token: 0x04003724 RID: 14116
		private const int MinTurrets = 2;

		// Token: 0x04003725 RID: 14117
		private const int MaxTurrets = 11;

		// Token: 0x04003726 RID: 14118
		private List<string> threatsTmp = new List<string>();
	}
}
