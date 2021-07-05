using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FEC RID: 4076
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x06005FFC RID: 24572 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x0020C080 File Offset: 0x0020A280
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			if (ManhunterPackGenStepUtility.TryGetAnimalsKind(sitePartParams.threatPoints, tile, out sitePartParams.animalKind))
			{
				sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, sitePartParams.animalKind.combatPower);
			}
			return sitePartParams;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x0020C0C8 File Offset: 0x0020A2C8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int animalsCount = this.GetAnimalsCount(part.parms);
			string output = GenLabel.BestKindLabel(part.parms.animalKind, Gender.None, true, animalsCount);
			outExtraDescriptionRules.Add(new Rule_String("count", animalsCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("kindLabel", output));
			outExtraDescriptionConstants.Add("count", animalsCount.ToString());
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x0020C13C File Offset: 0x0020A33C
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int animalsCount = this.GetAnimalsCount(sitePart.parms);
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(animalsCount.ToString(), GenLabel.BestKindLabel(sitePart.parms.animalKind, Gender.None, true, animalsCount));
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x0020C1A0 File Offset: 0x0020A3A0
		private int GetAnimalsCount(SitePartParams parms)
		{
			return ManhunterPackIncidentUtility.GetAnimalsCount(parms.animalKind, parms.threatPoints);
		}
	}
}
