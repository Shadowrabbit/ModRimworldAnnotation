using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000FE5 RID: 4069
	public class SitePartWorker_AncientComplex : SitePartWorker
	{
		// Token: 0x06005FEB RID: 24555 RVA: 0x0020BDC6 File Offset: 0x00209FC6
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
