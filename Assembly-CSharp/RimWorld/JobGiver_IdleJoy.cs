using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF8 RID: 3320
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x06004C53 RID: 19539 RVA: 0x0003638A File Offset: 0x0003458A
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

		// Token: 0x04003243 RID: 12867
		private const int GameStartNoIdleJoyTicks = 60000;
	}
}
