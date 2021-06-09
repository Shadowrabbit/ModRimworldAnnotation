using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191D RID: 6429
	public class Tradeable_Pawn : Tradeable
	{
		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x06008E65 RID: 36453 RVA: 0x0005F67C File Offset: 0x0005D87C
		public override Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.AnyPawn);
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x06008E66 RID: 36454 RVA: 0x002914D0 File Offset: 0x0028F6D0
		public override string Label
		{
			get
			{
				string text = base.Label;
				if (this.AnyPawn.Name != null && !this.AnyPawn.Name.Numerical)
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

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x06008E67 RID: 36455 RVA: 0x0005F689 File Offset: 0x0005D889
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

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x06008E68 RID: 36456 RVA: 0x0005F6BF File Offset: 0x0005D8BF
		private Pawn AnyPawn
		{
			get
			{
				return (Pawn)this.AnyThing;
			}
		}

		// Token: 0x06008E69 RID: 36457 RVA: 0x00291578 File Offset: 0x0028F778
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerSells)
			{
				List<Pawn> list = this.thingsColony.Take(base.CountToTransferToDestination).Cast<Pawn>().ToList<Pawn>();
				for (int i = 0; i < list.Count; i++)
				{
					TradeSession.trader.GiveSoldThingToTrader(list[i], 1, TradeSession.playerNegotiator);
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
