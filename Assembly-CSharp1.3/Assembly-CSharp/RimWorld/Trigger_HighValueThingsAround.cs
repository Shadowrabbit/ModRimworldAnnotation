using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E3 RID: 2275
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x06003BA4 RID: 15268 RVA: 0x0014C76C File Offset: 0x0014A96C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					return false;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					return num > num2;
				}
			}
			return false;
		}

		// Token: 0x04002070 RID: 8304
		private const int CheckInterval = 120;

		// Token: 0x04002071 RID: 8305
		private const int MinTicksSinceDamage = 300;
	}
}
