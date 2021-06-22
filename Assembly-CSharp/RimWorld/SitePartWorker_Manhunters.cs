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
    // Token: 0x020015B9 RID: 5561
    public class SitePartWorker_Manhunters : SitePartWorker
    {
        // Token: 0x060078BB RID: 30907 RVA: 0x0024ACC8 File Offset: 0x00248EC8
        public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef,
            out LookTargets lookTargets)
        {
            string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
            lookTargets = (from x in map.mapPawns.AllPawnsSpawned
                where x.MentalStateDef == MentalStateDefOf.Manhunter ||
                      x.MentalStateDef == MentalStateDefOf.ManhunterPermanent
                select x).FirstOrDefault<Pawn>();
            return arrivedLetterPart;
        }

        // Token: 0x060078BC RID: 30908 RVA: 0x0024AD1C File Offset: 0x00248F1C
        public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            if (ManhunterPackGenStepUtility.TryGetAnimalsKind(sitePartParams.threatPoints, tile,
                out sitePartParams.animalKind))
            {
                sitePartParams.threatPoints =
                    Mathf.Max(sitePartParams.threatPoints, sitePartParams.animalKind.combatPower);
            }

            return sitePartParams;
        }

        // Token: 0x060078BD RID: 30909 RVA: 0x0024AD64 File Offset: 0x00248F64
        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules,
            Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            int animalsCount = this.GetAnimalsCount(part.parms);
            string output = GenLabel.BestKindLabel(part.parms.animalKind, Gender.None, true, animalsCount);
            outExtraDescriptionRules.Add(new Rule_String("count", animalsCount.ToString()));
            outExtraDescriptionRules.Add(new Rule_String("kindLabel", output));
            outExtraDescriptionConstants.Add("count", animalsCount.ToString());
        }

        // Token: 0x060078BE RID: 30910 RVA: 0x0024ADD8 File Offset: 0x00248FD8
        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            int animalsCount = this.GetAnimalsCount(sitePart.parms);
            return base.GetPostProcessedThreatLabel(site, sitePart) + ": " +
                   "KnownSiteThreatEnemyCountAppend".Translate(animalsCount.ToString(),
                       GenLabel.BestKindLabel(sitePart.parms.animalKind, Gender.None, true, animalsCount));
        }

        // Token: 0x060078BF RID: 30911 RVA: 0x00051599 File Offset: 0x0004F799
        private int GetAnimalsCount(SitePartParams parms)
        {
            return ManhunterPackIncidentUtility.GetAnimalsCount(parms.animalKind, parms.threatPoints);
        }
    }
}