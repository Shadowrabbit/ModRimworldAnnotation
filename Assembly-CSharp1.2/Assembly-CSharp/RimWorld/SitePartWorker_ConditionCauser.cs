using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020015B4 RID: 5556
	public class SitePartWorker_ConditionCauser : SitePartWorker
	{
		// Token: 0x060078AC RID: 30892 RVA: 0x0024AB10 File Offset: 0x00248D10
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int worldRange = sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange;
			return base.GetPostProcessedThreatLabel(site, sitePart) + " (" + "ConditionCauserRadius".Translate(worldRange) + ")";
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x000514C4 File Offset: 0x0004F6C4
		public override void Init(Site site, SitePart sitePart)
		{
			sitePart.conditionCauser = ThingMaker.MakeThing(sitePart.def.conditionCauserDef, null);
			CompCauseGameCondition compCauseGameCondition = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition>();
			compCauseGameCondition.RandomizeSettings_NewTemp_NewTemp(site);
			compCauseGameCondition.LinkWithSite(sitePart.site);
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x000514FA File Offset: 0x0004F6FA
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			if (!sitePart.conditionCauser.DestroyedOrNull() && !sitePart.conditionCauser.Spawned)
			{
				sitePart.conditionCauser.Tick();
			}
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x00051521 File Offset: 0x0004F721
		public override void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
			base.PostDrawExtraSelectionOverlays(sitePart);
			GenDraw.DrawWorldRadiusRing(sitePart.site.Tile, sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x0024AB6C File Offset: 0x00248D6C
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
			{
				sitePart.conditionCauser.DeSpawn(DestroyMode.Vanish);
				sitePart.conditionCauserWasSpawned = false;
			}
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x0005154F File Offset: 0x0004F74F
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			slate.Set<Thing>("conditionCauser", part.conditionCauser, false);
			outExtraDescriptionRules.Add(new Rule_String("problemCauserLabel", part.conditionCauser.Label));
		}
	}
}
