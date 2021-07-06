using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191E RID: 6430
	public class Tradeable_RoyalFavor : Tradeable
	{
		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06008E6B RID: 36459 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06008E6C RID: 36460 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06008E6D RID: 36461 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06008E6E RID: 36462 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06008E6F RID: 36463 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06008E70 RID: 36464 RVA: 0x0000C32E File Offset: 0x0000A52E
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06008E71 RID: 36465 RVA: 0x0005F6D4 File Offset: 0x0005D8D4
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06008E72 RID: 36466 RVA: 0x0005F6EA File Offset: 0x0005D8EA
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		// Token: 0x06008E73 RID: 36467 RVA: 0x0005F70F File Offset: 0x0005D90F
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		// Token: 0x06008E74 RID: 36468 RVA: 0x0005F717 File Offset: 0x0005D917
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		// Token: 0x06008E75 RID: 36469 RVA: 0x00291620 File Offset: 0x0028F820
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f);
			GUI.color = Color.white;
		}

		// Token: 0x06008E76 RID: 36470 RVA: 0x0005F741 File Offset: 0x0005D941
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		// Token: 0x06008E77 RID: 36471 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
