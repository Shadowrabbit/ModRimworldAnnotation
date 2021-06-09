using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E30 RID: 3632
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x06005260 RID: 21088 RVA: 0x001BE250 File Offset: 0x001BC450
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

		// Token: 0x040034C8 RID: 13512
		private const int CheckInterval = 120;

		// Token: 0x040034C9 RID: 13513
		private const int MinTicksSinceDamage = 300;
	}
}
