using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B13 RID: 2835
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x06004241 RID: 16961 RVA: 0x00189518 File Offset: 0x00187718
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
