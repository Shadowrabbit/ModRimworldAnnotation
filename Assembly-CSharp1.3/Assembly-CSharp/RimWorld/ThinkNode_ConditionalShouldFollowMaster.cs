using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200090C RID: 2316
	public class ThinkNode_ConditionalShouldFollowMaster : ThinkNode_Conditional
	{
		// Token: 0x06003C4A RID: 15434 RVA: 0x0014F433 File Offset: 0x0014D633
		protected override bool Satisfied(Pawn pawn)
		{
			return ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0014F43C File Offset: 0x0014D63C
		public static bool ShouldFollowMaster(Pawn pawn)
		{
			if (!pawn.Spawned || pawn.playerSettings == null)
			{
				return false;
			}
			Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
			if (respectedMaster == null)
			{
				return false;
			}
			if (respectedMaster.Spawned)
			{
				if (pawn.playerSettings.followDrafted && respectedMaster.Drafted && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
				if (pawn.playerSettings.followFieldwork && respectedMaster.mindState.lastJobTag == JobTag.Fieldwork && pawn.CanReach(respectedMaster, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			else
			{
				Pawn carriedBy = respectedMaster.CarriedBy;
				if (carriedBy != null && carriedBy.HostileTo(respectedMaster) && pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
