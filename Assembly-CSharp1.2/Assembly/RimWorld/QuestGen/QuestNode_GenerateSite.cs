using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0C RID: 7948
	public class QuestNode_GenerateSite : QuestNode
	{
		// Token: 0x0600AA25 RID: 43557 RVA: 0x0031AE1C File Offset: 0x0031901C
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficultyValues.allowViolentQuests && this.sitePartsParams.GetValue(slate) != null)
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

		// Token: 0x0600AA26 RID: 43558 RVA: 0x0031AE9C File Offset: 0x0031909C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Site var = QuestGen_Sites.GenerateSite(this.sitePartsParams.GetValue(slate), this.tile.GetValue(slate), this.faction.GetValue(slate), this.hiddenSitePartsPossible.GetValue(slate), this.singleSitePartRules.GetValue(slate));
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Site>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x04007385 RID: 29573
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;

		// Token: 0x04007386 RID: 29574
		public SlateRef<Faction> faction;

		// Token: 0x04007387 RID: 29575
		public SlateRef<int> tile;

		// Token: 0x04007388 RID: 29576
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007389 RID: 29577
		public SlateRef<RulePack> singleSitePartRules;

		// Token: 0x0400738A RID: 29578
		public SlateRef<bool> hiddenSitePartsPossible;

		// Token: 0x0400738B RID: 29579
		private const string RootSymbol = "root";
	}
}
