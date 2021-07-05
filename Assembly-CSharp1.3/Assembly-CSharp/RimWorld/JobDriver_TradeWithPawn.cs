using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000738 RID: 1848
	public class JobDriver_TradeWithPawn : JobDriver
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x00124C4F File Offset: 0x00122E4F
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x00124C5C File Offset: 0x00122E5C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00124C7E File Offset: 0x00122E7E
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
