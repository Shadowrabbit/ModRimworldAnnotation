using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001236 RID: 4662
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06006FEA RID: 28650 RVA: 0x00254F99 File Offset: 0x00253199
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn, null);
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06006FEB RID: 28651 RVA: 0x00254FA8 File Offset: 0x002531A8
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical && !this.AnyPawn.RaceProps.Humanlike)
				{
					text = text + ", " + this.AnyPawn.def.label;
				}
				return string.Concat(new string[]
				{
					text,
					" (",
					this.AnyPawn.GetGenderLabel(),
					", ",
					Mathf.FloorToInt(this.AnyPawn.ageTracker.AgeBiologicalYearsFloat).ToString(),
					")"
				});
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06006FEC RID: 28652 RVA: 0x00255060 File Offset: 0x00253260
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyPawn.MainDesc(true) + "\n\n" + this.AnyPawn.def.description;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06006FED RID: 28653 RVA: 0x00255096 File Offset: 0x00253296
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x002550A4 File Offset: 0x002532A4
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					bool flag = GuestUtility.IsSellingToSlavery(list[i], TradeSession.trader.Faction);
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
					if (flag)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer)), true);
					}
				}
				return;
			}
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				List<Pawn> list2 = this.thingsTrader.Take(base.CountToTransferToSource).Cast<Pawn>().ToList<Pawn>();
				for (int j = 0; j < list2.Count; j++)
				{
					TradeSession.trader.GiveSoldThingToPlayer(list2[j], 1, TradeSession.playerNegotiator);
				}
			}
		}
	}
}
