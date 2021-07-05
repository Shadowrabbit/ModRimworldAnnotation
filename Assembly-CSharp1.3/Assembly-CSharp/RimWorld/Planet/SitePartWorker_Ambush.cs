using System;

namespace RimWorld.Planet
{
	// Token: 0x020017DA RID: 6106
	public class SitePartWorker_Ambush : SitePartWorker
	{
		// Token: 0x06008E37 RID: 36407 RVA: 0x003314B0 File Offset: 0x0032F6B0
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		// Token: 0x040059D8 RID: 23000
		private const float ThreatPointsFactor = 0.8f;
	}
}
