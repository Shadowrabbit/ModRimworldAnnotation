using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000FF2 RID: 4082
	public static class SleepingMechanoidsSitePartUtility
	{
		// Token: 0x06006019 RID: 24601 RVA: 0x0020C407 File Offset: 0x0020A607
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
