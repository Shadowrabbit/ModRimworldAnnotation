using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001237 RID: 4663
	public class Tradeable_RoyalFavor : Tradeable
	{
		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06006FF1 RID: 28657 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x06006FF3 RID: 28659 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06006FF4 RID: 28660 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06006FF5 RID: 28661 RVA: 0x00002688 File Offset: 0x00000888
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06006FF6 RID: 28662 RVA: 0x00255193 File Offset: 0x00253393
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06006FF7 RID: 28663 RVA: 0x002551A9 File Offset: 0x002533A9
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		// Token: 0x06006FF8 RID: 28664 RVA: 0x002551CE File Offset: 0x002533CE
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		// Token: 0x06006FF9 RID: 28665 RVA: 0x002551D6 File Offset: 0x002533D6
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		// Token: 0x06006FFA RID: 28666 RVA: 0x00255200 File Offset: 0x00253400
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f);
			GUI.color = Color.white;
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x00255243 File Offset: 0x00253443
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		// Token: 0x06006FFC RID: 28668 RVA: 0x0001276E File Offset: 0x0001096E
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
