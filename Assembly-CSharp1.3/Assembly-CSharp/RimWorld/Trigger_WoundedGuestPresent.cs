using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E6 RID: 2278
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003BAB RID: 15275 RVA: 0x0014C7C9 File Offset: 0x0014A9C9
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x0014C7D6 File Offset: 0x0014A9D6
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0014C8E4 File Offset: 0x0014AAE4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				if (lord.ownedPawns.Any<Pawn>())
				{
					Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState && KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002075 RID: 8309
		private const int CheckInterval = 800;
	}
}
