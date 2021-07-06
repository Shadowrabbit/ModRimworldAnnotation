using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020014D7 RID: 5335
	public static class RestraintsUtility
	{
		// Token: 0x060072F9 RID: 29433 RVA: 0x00232070 File Offset: 0x00230270
		public static bool InRestraints(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			if (pawn.HostFaction == null)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			return (lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released);
		}

		// Token: 0x060072FA RID: 29434 RVA: 0x0004D51D File Offset: 0x0004B71D
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
