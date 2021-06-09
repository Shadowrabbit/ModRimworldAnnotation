using System;

namespace RimWorld.Planet
{
	// Token: 0x02002167 RID: 8551
	public class SitePartWorker_Ambush : SitePartWorker
	{
		// Token: 0x0600B63B RID: 46651 RVA: 0x00076410 File Offset: 0x00074610
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		// Token: 0x04007CD6 RID: 31958
		private const float ThreatPointsFactor = 0.8f;
	}
}
