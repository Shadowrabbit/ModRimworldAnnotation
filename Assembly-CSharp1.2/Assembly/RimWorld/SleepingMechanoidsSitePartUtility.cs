using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015C2 RID: 5570
	public static class SleepingMechanoidsSitePartUtility
	{
		// Token: 0x060078E0 RID: 30944 RVA: 0x0005164B File Offset: 0x0004F84B
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
