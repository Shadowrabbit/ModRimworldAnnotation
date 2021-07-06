using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F19 RID: 7961
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
	{
		// Token: 0x0600AA52 RID: 43602 RVA: 0x0006F9EC File Offset: 0x0006DBEC
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA53 RID: 43603 RVA: 0x0006F9F6 File Offset: 0x0006DBF6
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA54 RID: 43604 RVA: 0x0031B490 File Offset: 0x00319690
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

		// Token: 0x040073AE RID: 29614
		public SlateRef<int> tile;

		// Token: 0x040073AF RID: 29615
		public SlateRef<Faction> faction;

		// Token: 0x040073B0 RID: 29616
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x040073B1 RID: 29617
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
