using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0A RID: 3082
	public class JobDriver_TradeWithPawn : JobDriver
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x0003490F File Offset: 0x00032B0F
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0003491C File Offset: 0x00032B1C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0003493E File Offset: 0x00032B3E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Trader.CanTradeNow);
			Toil trade = new Toil();
			trade.initAction = delegate()
			{
				Pawn actor = trade.actor;
				if (this.Trader.CanTradeNow)
				{
					Find.WindowStack.Add(new Dialog_Trade(actor, this.Trader, false));
				}
			};
			yield return trade;
			yield break;
		}
	}
}
