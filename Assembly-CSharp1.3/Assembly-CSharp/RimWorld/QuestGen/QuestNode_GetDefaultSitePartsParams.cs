using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166C RID: 5740
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
	{
		// Token: 0x060085B4 RID: 34228 RVA: 0x002FF45F File Offset: 0x002FD65F
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085B5 RID: 34229 RVA: 0x002FF469 File Offset: 0x002FD669
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085B6 RID: 34230 RVA: 0x002FF478 File Offset: 0x002FD678
		private void SetVars(Slate slate)
		{
			List<SitePartDefWithParams> list;
			SiteMakerHelper.GenerateDefaultParams(slate.Get<float>("points", 0f, false), this.tile.GetValue(slate), this.faction.GetValue(slate), this.sitePartDefs.GetValue(slate), out list);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == SitePartDefOf.PreciousLump)
				{
					list[i].parms.preciousLumpResources = slate.Get<ThingDef>("targetMineable", null, false);
				}
			}
			slate.Set<List<SitePartDefWithParams>>(this.storeSitePartsParamsAs.GetValue(slate), list, false);
		}

		// Token: 0x04005384 RID: 21380
		public SlateRef<int> tile;

		// Token: 0x04005385 RID: 21381
		public SlateRef<Faction> faction;

		// Token: 0x04005386 RID: 21382
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04005387 RID: 21383
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
