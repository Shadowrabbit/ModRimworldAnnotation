using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015BE RID: 5566
	public static class OutpostSitePartUtility
	{
		// Token: 0x060078D1 RID: 30929 RVA: 0x0005164B File Offset: 0x0004F84B
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
