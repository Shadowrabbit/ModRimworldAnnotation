using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001691 RID: 5777
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		// Token: 0x06008654 RID: 34388 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008655 RID: 34389 RVA: 0x00302A48 File Offset: 0x00300C48
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

		// Token: 0x0400541C RID: 21532
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400541D RID: 21533
		public SlateRef<Site> site;

		// Token: 0x0400541E RID: 21534
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
