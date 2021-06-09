using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160A RID: 5642
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06007AB2 RID: 31410 RVA: 0x0005275D File Offset: 0x0005095D
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			if (method == PlayerPawnsArriveMethod.Standing)
			{
				return "PlayerPawnsArriveMethod_Standing".Translate();
			}
			if (method != PlayerPawnsArriveMethod.DropPods)
			{
				throw new NotImplementedException();
			}
			return "PlayerPawnsArriveMethod_DropPods".Translate();
		}
	}
}
