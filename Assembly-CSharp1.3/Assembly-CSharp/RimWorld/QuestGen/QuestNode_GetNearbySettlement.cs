using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200167C RID: 5756
	public class QuestNode_GetNearbySettlement : QuestNode
	{
		// Token: 0x060085FB RID: 34299 RVA: 0x00300BBC File Offset: 0x002FEDBC
		private Settlement RandomNearbyTradeableSettlement(int originTile, Slate slate)
		{
			return Find.WorldObjects.SettlementBases.Where(delegate(Settlement settlement)
			{
				if (!settlement.Visitable)
				{
					return false;
				}
				if (!this.allowActiveTradeRequest.GetValue(slate))
				{
					if (settlement.GetComponent<TradeRequestComp>() != null && settlement.GetComponent<TradeRequestComp>().ActiveRequest)
					{
						return false;
					}
					List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
					for (int i = 0; i < questsListForReading.Count; i++)
					{
						if (!questsListForReading[i].Historical)
						{
							List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
							for (int j = 0; j < partsListForReading.Count; j++)
							{
								QuestPart_InitiateTradeRequest questPart_InitiateTradeRequest;
								if ((questPart_InitiateTradeRequest = (partsListForReading[j] as QuestPart_InitiateTradeRequest)) != null && questPart_InitiateTradeRequest.settlement == settlement)
								{
									return false;
								}
							}
						}
					}
				}
				return Find.WorldGrid.ApproxDistanceInTiles(originTile, settlement.Tile) < this.maxTileDistance.GetValue(slate) && Find.WorldReachability.CanReach(originTile, settlement.Tile);
			}).RandomElementWithFallback(null);
		}

		// Token: 0x060085FC RID: 34300 RVA: 0x00300C08 File Offset: 0x002FEE08
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			QuestGen.slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
			if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
			{
				QuestGen.slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
			}
			if (!this.storeFactionLeaderAs.GetValue(slate).NullOrEmpty())
			{
				QuestGen.slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
			}
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x00300CB8 File Offset: 0x002FEEB8
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			if (map != null && settlement != null)
			{
				slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
				if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
				{
					slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
				}
				if (!string.IsNullOrEmpty(this.storeFactionLeaderAs.GetValue(slate)))
				{
					slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040053C1 RID: 21441
		public SlateRef<bool> allowActiveTradeRequest = true;

		// Token: 0x040053C2 RID: 21442
		public SlateRef<float> maxTileDistance;

		// Token: 0x040053C3 RID: 21443
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053C4 RID: 21444
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x040053C5 RID: 21445
		[NoTranslate]
		public SlateRef<string> storeFactionLeaderAs;
	}
}
