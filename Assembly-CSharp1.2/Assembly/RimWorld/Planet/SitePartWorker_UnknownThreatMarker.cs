using System;

namespace RimWorld.Planet
{
	// Token: 0x0200216C RID: 8556
	public class SitePartWorker_UnknownThreatMarker : SitePartWorker
	{
		// Token: 0x0600B64A RID: 46666 RVA: 0x00076447 File Offset: 0x00074647
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
