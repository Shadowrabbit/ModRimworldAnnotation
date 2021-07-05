using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001650 RID: 5712
	public class QuestNode_GenerateSite : QuestNode
	{
		// Token: 0x06008558 RID: 34136 RVA: 0x002FE0D4 File Offset: 0x002FC2D4
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests && this.sitePartsParams.GetValue(slate) != null)
			{
				using (IEnumerator<SitePartDefWithParams> enumerator = this.sitePartsParams.GetValue(slate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.wantsThreatPoints)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x002FE154 File Offset: 0x002FC354
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Site var = QuestGen_Sites.GenerateSite(this.sitePartsParams.GetValue(slate), this.tile.GetValue(slate), this.faction.GetValue(slate), this.hiddenSitePartsPossible.GetValue(slate), this.singleSitePartRules.GetValue(slate));
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Site>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x04005330 RID: 21296
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;

		// Token: 0x04005331 RID: 21297
		public SlateRef<Faction> faction;

		// Token: 0x04005332 RID: 21298
		public SlateRef<int> tile;

		// Token: 0x04005333 RID: 21299
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005334 RID: 21300
		public SlateRef<RulePack> singleSitePartRules;

		// Token: 0x04005335 RID: 21301
		public SlateRef<bool> hiddenSitePartsPossible;

		// Token: 0x04005336 RID: 21302
		private const string RootSymbol = "root";
	}
}
