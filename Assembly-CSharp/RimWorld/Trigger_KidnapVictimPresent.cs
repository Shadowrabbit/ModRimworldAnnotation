using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E31 RID: 3633
	public class Trigger_KidnapVictimPresent : Trigger
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x000399DC File Offset: 0x00037BDC
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x000399E9 File Offset: 0x00037BE9
		public Trigger_KidnapVictimPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001BE2B0 File Offset: 0x001BC4B0
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

		// Token: 0x040034CA RID: 13514
		private const int CheckInterval = 120;

		// Token: 0x040034CB RID: 13515
		private const int MinTicksSinceDamage = 300;
	}
}
