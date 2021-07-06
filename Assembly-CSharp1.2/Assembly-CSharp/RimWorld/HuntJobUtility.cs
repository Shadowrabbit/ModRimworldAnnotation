using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCE RID: 3022
	public static class HuntJobUtility
	{
		// Token: 0x06004715 RID: 18197 RVA: 0x0019717C File Offset: 0x0019537C
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
