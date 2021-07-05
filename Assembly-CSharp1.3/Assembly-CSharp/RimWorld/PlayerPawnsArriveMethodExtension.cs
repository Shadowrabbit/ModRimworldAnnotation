using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100F RID: 4111
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x060060EA RID: 24810 RVA: 0x0020F19D File Offset: 0x0020D39D
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
