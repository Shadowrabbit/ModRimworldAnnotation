using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C0 RID: 6080
	public static class TilesPerDayCalculator
	{
		// Token: 0x06008D0A RID: 36106 RVA: 0x0032C46C File Offset: 0x0032A66C
		public static float ApproxTilesPerDay(int caravanTicksPerMove, int tile, int nextTile, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
		{
			if (nextTile == -1)
			{
				nextTile = Find.WorldGrid.FindMostReasonableAdjacentTileForDisplayedPathCost(tile);
			}
			int num = Mathf.CeilToInt((float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, tile, nextTile, null, false, explanation, caravanTicksPerMoveExplanation, immobile) / 1f);
			if (num == 0)
			{
				return 0f;
			}
			return 60000f / (float)num;
		}

		// Token: 0x06008D0B RID: 36107 RVA: 0x0032C4C0 File Offset: 0x0032A6C0
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, caravan.pather.Moving ? caravan.pather.nextTile : -1, explanation, (explanation != null) ? caravan.TicksPerMoveExplanation : null, caravan.ImmobilizedByMass);
		}

		// Token: 0x06008D0C RID: 36108 RVA: 0x0032C50C File Offset: 0x0032A70C
		public static float ApproxTilesPerDay(List<TransferableOneWay> transferables, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06008D0D RID: 36109 RVA: 0x0032C5D0 File Offset: 0x0032A7D0
		public static float ApproxTilesPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, float massUsageLeftAfterTransfer, float massCapacityLeftAfterTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsageLeftAfterTransfer, massCapacityLeftAfterTransfer, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsageLeftAfterTransfer > massCapacityLeftAfterTransfer);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06008D0E RID: 36110 RVA: 0x0032C69D File Offset: 0x0032A89D
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06008D0F RID: 36111 RVA: 0x0032C6D4 File Offset: 0x0032A8D4
		public static float ApproxTilesPerDay(List<ThingCount> thingCounts, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						TilesPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x04005974 RID: 22900
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04005975 RID: 22901
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();
	}
}
