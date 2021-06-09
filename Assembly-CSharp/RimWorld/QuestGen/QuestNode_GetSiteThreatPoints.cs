using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F5D RID: 8029
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		// Token: 0x0600AB44 RID: 43844 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AB45 RID: 43845 RVA: 0x0031E9F0 File Offset: 0x0031CBF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.site.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeAs.GetValue(slate), this.site.GetValue(slate).ActualThreatPoints, false);
				return;
			}
			float num = 0f;
			IEnumerable<SitePartDefWithParams> value = this.sitePartsParams.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in value)
				{
					num += sitePartDefWithParams.parms.threatPoints;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04007495 RID: 29845
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007496 RID: 29846
		public SlateRef<Site> site;

		// Token: 0x04007497 RID: 29847
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
