using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DE RID: 2014
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x06003610 RID: 13840 RVA: 0x0013212C File Offset: 0x0013032C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < 60000)
			{
				return null;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn) || JoyUtility.TimetablePreventsGettingJoy(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x04001ECE RID: 7886
		private const int GameStartNoIdleJoyTicks = 60000;
	}
}
