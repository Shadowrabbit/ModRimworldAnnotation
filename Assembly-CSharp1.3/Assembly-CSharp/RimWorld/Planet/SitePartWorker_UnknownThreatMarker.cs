using System;

namespace RimWorld.Planet
{
	// Token: 0x020017DF RID: 6111
	public class SitePartWorker_UnknownThreatMarker : SitePartWorker
	{
		// Token: 0x06008E46 RID: 36422 RVA: 0x00331A83 File Offset: 0x0032FC83
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == SitePartDefOf.PreciousLump)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
