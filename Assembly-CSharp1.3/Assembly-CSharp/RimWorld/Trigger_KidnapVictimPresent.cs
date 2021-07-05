using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E4 RID: 2276
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x0014C7C9 File Offset: 0x0014A9C9
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x0014C7D6 File Offset: 0x0014A9D6
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x0014C7EC File Offset: 0x0014A9EC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (this.data == null || !(this.data is TriggerData_PawnCycleInd))
				{
					BackCompatibility.TriggerDataPawnCycleIndNull(this);
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
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
						Pawn pawn2;
						if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null && KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, null) && !GenAI.InDangerousCombat(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04002072 RID: 8306
		private const int CheckInterval = 120;

		// Token: 0x04002073 RID: 8307
		private const int MinTicksSinceDamage = 300;
	}
}
