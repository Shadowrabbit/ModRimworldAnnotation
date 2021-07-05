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
    // Token: 0x020015C3 RID: 5571
    public class SitePartWorker_Turrets : SitePartWorker
    {
        // Token: 0x060078E1 RID: 30945 RVA: 0x0024B3FC File Offset: 0x002495FC
        public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
        {
            string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
            Thing t;
            if ((t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun && x.HostileTo(Faction.OfPlayer))) ==
                null)
            {
                t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun);
            }
            lookTargets = t;
            return arrivedLetterPart;
        }

        // Token: 0x060078E2 RID: 30946 RVA: 0x0024B47C File Offset: 0x0024967C
        public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
            sitePartParams.turretsCount =
                Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
            return sitePartParams;
        }

        // Token: 0x060078E3 RID: 30947 RVA: 0x0024B4C8 File Offset: 0x002496C8
        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules,
            Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            string threatsInfo_NewTmp = this.GetThreatsInfo_NewTmp(part.parms, part.site.Faction);
            outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo_NewTmp));
        }

        // Token: 0x060078E4 RID: 30948 RVA: 0x000516BE File Offset: 0x0004F8BE
        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo_NewTmp(sitePart.parms, site.Faction);
        }

        // Token: 0x060078E5 RID: 30949 RVA: 0x000516E4 File Offset: 0x0004F8E4
        [Obsolete]
        private string GetThreatsInfo(SitePartParams parms)
        {
            return this.GetThreatsInfo_NewTmp(parms, null);
        }

        // Token: 0x060078E6 RID: 30950 RVA: 0x0024B50C File Offset: 0x0024970C
        private string GetThreatsInfo_NewTmp(SitePartParams parms, Faction faction)
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
            return this.threatsTmp.ToCommaList(true);
        }

        // Token: 0x04004F90 RID: 20368
        private const int MinTurrets = 2;

        // Token: 0x04004F91 RID: 20369
        private const int MaxTurrets = 11;

        // Token: 0x04004F92 RID: 20370
        private List<string> threatsTmp = new List<string>();
    }
}
