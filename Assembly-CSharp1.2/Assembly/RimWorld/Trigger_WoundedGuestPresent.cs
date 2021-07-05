using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E33 RID: 3635
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x000399DC File Offset: 0x00037BDC
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x000399E9 File Offset: 0x00037BE9
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x001BE394 File Offset: 0x001BC594
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

		// Token: 0x040034CD RID: 13517
		private const int CheckInterval = 800;
	}
}
