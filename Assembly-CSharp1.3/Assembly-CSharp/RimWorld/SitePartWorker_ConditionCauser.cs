using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE7 RID: 4071
	public class SitePartWorker_ConditionCauser : SitePartWorker
	{
		// Token: 0x06005FED RID: 24557 RVA: 0x0020BDE8 File Offset: 0x00209FE8
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int worldRange = sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange;
			return base.GetPostProcessedThreatLabel(site, sitePart) + " (" + "ConditionCauserRadius".Translate(worldRange) + ")";
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x0020BE41 File Offset: 0x0020A041
		public override void Init(Site site, SitePart sitePart)
		{
			sitePart.conditionCauser = ThingMaker.MakeThing(sitePart.def.conditionCauserDef, null);
			CompCauseGameCondition compCauseGameCondition = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition>();
			compCauseGameCondition.RandomizeSettings(site);
			compCauseGameCondition.LinkWithSite(sitePart.site);
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x0020BE77 File Offset: 0x0020A077
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			if (!sitePart.conditionCauser.DestroyedOrNull() && !sitePart.conditionCauser.Spawned)
			{
				sitePart.conditionCauser.Tick();
			}
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x0020BE9E File Offset: 0x0020A09E
		public override void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
			base.PostDrawExtraSelectionOverlays(sitePart);
			GenDraw.DrawWorldRadiusRing(sitePart.site.Tile, sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x0020BECC File Offset: 0x0020A0CC
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
			{
				sitePart.conditionCauser.DeSpawn(DestroyMode.Vanish);
				sitePart.conditionCauserWasSpawned = false;
			}
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x0020BF25 File Offset: 0x0020A125
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			slate.Set<Thing>("conditionCauser", part.conditionCauser, false);
			outExtraDescriptionRules.Add(new Rule_String("problemCauserLabel", part.conditionCauser.Label));
		}
	}
}
