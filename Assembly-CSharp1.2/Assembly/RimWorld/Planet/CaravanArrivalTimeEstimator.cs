using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020DD RID: 8413
	public static class CaravanArrivalTimeEstimator
	{
		// Token: 0x0600B2C6 RID: 45766 RVA: 0x0033CCB8 File Offset: 0x0033AEB8
		public static int EstimatedTicksToArrive(Caravan caravan, bool allowCaching)
		{
			if (allowCaching && caravan == CaravanArrivalTimeEstimator.cachedForCaravan && caravan.pather.Destination == CaravanArrivalTimeEstimator.cachedForDest && Find.TickManager.TicksGame - CaravanArrivalTimeEstimator.cacheTicks < 100)
			{
				return CaravanArrivalTimeEstimator.cachedResult;
			}
			int to;
			int result;
			if (!caravan.Spawned || !caravan.pather.Moving || caravan.pather.curPath == null)
			{
				to = -1;
				result = 0;
			}
			else
			{
				to = caravan.pather.Destination;
				result = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(caravan.Tile, to, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, Find.TickManager.TicksAbs);
			}
			if (allowCaching)
			{
				CaravanArrivalTimeEstimator.cacheTicks = Find.TickManager.TicksGame;
				CaravanArrivalTimeEstimator.cachedForCaravan = caravan;
				CaravanArrivalTimeEstimator.cachedForDest = to;
				CaravanArrivalTimeEstimator.cachedResult = result;
			}
			return result;
		}

		// Token: 0x0600B2C7 RID: 45767 RVA: 0x0033CD88 File Offset: 0x0033AF88
		public static int EstimatedTicksToArrive(int from, int to, Caravan caravan)
		{
			int result;
			using (WorldPath worldPath = Find.WorldPathFinder.FindPath(from, to, caravan, null))
			{
				if (!worldPath.Found)
				{
					result = 0;
				}
				else
				{
					result = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(from, to, worldPath, 0f, (caravan != null) ? caravan.TicksPerMove : 3300, Find.TickManager.TicksAbs);
				}
			}
			return result;
		}

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x000742D4 File Offset: 0x000724D4
		public static int EstimatedTicksToArrive(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs)
		{
			CaravanArrivalTimeEstimator.tmpTicksToArrive.Clear();
			CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(from, to, path, nextTileCostLeft, caravanTicksPerMove, curTicksAbs, CaravanArrivalTimeEstimator.tmpTicksToArrive);
			return CaravanArrivalTimeEstimator.EstimatedTicksToArrive(to, CaravanArrivalTimeEstimator.tmpTicksToArrive);
		}

		// Token: 0x0600B2C9 RID: 45769 RVA: 0x0033CDF8 File Offset: 0x0033AFF8
		public static void EstimatedTicksToArriveToEvery(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs, List<Pair<int, int>> outTicksToArrive)
		{
			outTicksToArrive.Clear();
			outTicksToArrive.Add(new Pair<int, int>(from, 0));
			if (from == to)
			{
				outTicksToArrive.Add(new Pair<int, int>(to, 0));
				return;
			}
			int num = 0;
			int num2 = from;
			int num3 = 0;
			int num4 = Mathf.CeilToInt(20000f) - 1;
			int num5 = 60000 - num4;
			int num6 = 0;
			int num7 = 0;
			int num9;
			if (CaravanNightRestUtility.WouldBeRestingAt(from, (long)curTicksAbs))
			{
				if (Caravan_PathFollower.IsValidFinalPushDestination(to) && (path.Peek(0) == to || (nextTileCostLeft <= 0f && path.NodesLeftCount >= 2 && path.Peek(1) == to)))
				{
					int num8 = Mathf.CeilToInt(CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, path.Peek(0) == to, curTicksAbs, num, caravanTicksPerMove, from, to) / 1f);
					if (num8 <= 10000)
					{
						num += num8;
						outTicksToArrive.Add(new Pair<int, int>(to, num));
						return;
					}
				}
				num += CaravanNightRestUtility.LeftRestTicksAt(from, (long)curTicksAbs);
				num9 = num5;
			}
			else
			{
				num9 = CaravanNightRestUtility.LeftNonRestTicksAt(from, (long)curTicksAbs);
			}
			for (;;)
			{
				num7++;
				if (num7 >= 10000)
				{
					break;
				}
				if (num6 <= 0)
				{
					if (num2 == to)
					{
						goto Block_10;
					}
					bool firstInPath = num3 == 0;
					int num10 = num2;
					num2 = path.Peek(num3);
					num3++;
					outTicksToArrive.Add(new Pair<int, int>(num10, num));
					num6 = Mathf.CeilToInt(CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, firstInPath, curTicksAbs, num, caravanTicksPerMove, num10, num2) / 1f);
				}
				if (num9 < num6)
				{
					num += num9;
					num6 -= num9;
					if (num2 == to && num6 <= 10000 && Caravan_PathFollower.IsValidFinalPushDestination(to))
					{
						goto Block_14;
					}
					num += num4;
					num9 = num5;
				}
				else
				{
					num += num6;
					num9 -= num6;
					num6 = 0;
				}
			}
			Log.ErrorOnce("Could not calculate estimated ticks to arrive. Too many iterations.", 1837451324, false);
			outTicksToArrive.Add(new Pair<int, int>(to, num));
			return;
			Block_10:
			outTicksToArrive.Add(new Pair<int, int>(to, num));
			return;
			Block_14:
			num += num6;
			outTicksToArrive.Add(new Pair<int, int>(to, num));
		}

		// Token: 0x0600B2CA RID: 45770 RVA: 0x0033CFCC File Offset: 0x0033B1CC
		private static float GetCostToMove(float initialNextTileCostLeft, bool firstInPath, int initialTicksAbs, int curResult, int caravanTicksPerMove, int curTile, int nextTile)
		{
			if (firstInPath)
			{
				return initialNextTileCostLeft;
			}
			int value = initialTicksAbs + curResult;
			return (float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, curTile, nextTile, new int?(value), false, null, null);
		}

		// Token: 0x0600B2CB RID: 45771 RVA: 0x0033CFF8 File Offset: 0x0033B1F8
		public static int EstimatedTicksToArrive(int destinationTile, List<Pair<int, int>> estimatedTicksToArriveToEvery)
		{
			if (destinationTile == -1)
			{
				return 0;
			}
			for (int i = 0; i < estimatedTicksToArriveToEvery.Count; i++)
			{
				if (destinationTile == estimatedTicksToArriveToEvery[i].First)
				{
					return estimatedTicksToArriveToEvery[i].Second;
				}
			}
			return 0;
		}

		// Token: 0x0600B2CC RID: 45772 RVA: 0x0033D040 File Offset: 0x0033B240
		public static int TileIllBeInAt(int ticksAbs, List<Pair<int, int>> estimatedTicksToArriveToEvery, int ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery)
		{
			if (!estimatedTicksToArriveToEvery.Any<Pair<int, int>>())
			{
				return -1;
			}
			for (int i = estimatedTicksToArriveToEvery.Count - 1; i >= 0; i--)
			{
				int num = ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery + estimatedTicksToArriveToEvery[i].Second;
				if (ticksAbs >= num)
				{
					return estimatedTicksToArriveToEvery[i].First;
				}
			}
			return estimatedTicksToArriveToEvery[0].First;
		}

		// Token: 0x04007AEB RID: 31467
		private static int cacheTicks = -1;

		// Token: 0x04007AEC RID: 31468
		private static Caravan cachedForCaravan;

		// Token: 0x04007AED RID: 31469
		private static int cachedForDest = -1;

		// Token: 0x04007AEE RID: 31470
		private static int cachedResult = -1;

		// Token: 0x04007AEF RID: 31471
		private const int CacheDuration = 100;

		// Token: 0x04007AF0 RID: 31472
		private const int MaxIterations = 10000;

		// Token: 0x04007AF1 RID: 31473
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();
	}
}
