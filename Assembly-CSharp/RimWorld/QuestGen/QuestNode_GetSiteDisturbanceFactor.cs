using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F58 RID: 8024
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		// Token: 0x0600AB33 RID: 43827 RVA: 0x0007003C File Offset: 0x0006E23C
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AB34 RID: 43828 RVA: 0x00070046 File Offset: 0x0006E246
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AB35 RID: 43829 RVA: 0x0031E6B8 File Offset: 0x0031C8B8
		private void SetVars(Slate slate)
		{
			float num = 1f;
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDef sitePartDef in value)
				{
					num *= sitePartDef.activeThreatDisturbanceFactor;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04007484 RID: 29828
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007485 RID: 29829
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
