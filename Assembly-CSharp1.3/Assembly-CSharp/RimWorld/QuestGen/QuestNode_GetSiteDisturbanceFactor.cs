using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168F RID: 5775
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		// Token: 0x0600864B RID: 34379 RVA: 0x0030277D File Offset: 0x0030097D
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600864C RID: 34380 RVA: 0x00302787 File Offset: 0x00300987
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600864D RID: 34381 RVA: 0x00302794 File Offset: 0x00300994
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

		// Token: 0x04005415 RID: 21525
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005416 RID: 21526
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
