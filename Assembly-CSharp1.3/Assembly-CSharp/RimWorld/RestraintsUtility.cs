using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E37 RID: 3639
	public static class RestraintsUtility
	{
		// Token: 0x06005440 RID: 21568 RVA: 0x001C8A8C File Offset: 0x001C6C8C
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
			return (lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released) && !pawn.IsSlave;
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x001C8AF0 File Offset: 0x001C6CF0
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
