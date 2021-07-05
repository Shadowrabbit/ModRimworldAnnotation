using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006B1 RID: 1713
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x06002F6F RID: 12143 RVA: 0x00119030 File Offset: 0x00117230
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.IncappedOrKilled || signal.condition == PawnLostCondition.MadePrisoner))
			{
				if (signal.Pawn.GetTraderCaravanRole() == TraderCaravanRole.Trader || signal.Pawn.RaceProps.packAnimal)
				{
					return true;
				}
				if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
