using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071C RID: 1820
	public static class HuntJobUtility
	{
		// Token: 0x06003294 RID: 12948 RVA: 0x001230A8 File Offset: 0x001212A8
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return false;
			}
			Pawn pawn2 = dinfo.Value.Instigator as Pawn;
			if (pawn2 == null || pawn2.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn2.jobs.curDriver as JobDriver_Hunt;
			return jobDriver_Hunt != null && jobDriver_Hunt.Victim == pawn;
		}
	}
}
