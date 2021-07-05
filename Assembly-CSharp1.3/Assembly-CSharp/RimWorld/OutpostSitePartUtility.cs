using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000FEF RID: 4079
	public static class OutpostSitePartUtility
	{
		// Token: 0x0600600D RID: 24589 RVA: 0x0020C407 File Offset: 0x0020A607
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
