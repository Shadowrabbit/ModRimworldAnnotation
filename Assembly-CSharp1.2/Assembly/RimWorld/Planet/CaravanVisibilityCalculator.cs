using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002105 RID: 8453
	public static class CaravanVisibilityCalculator
	{
		// Token: 0x0600B381 RID: 45953 RVA: 0x003405E8 File Offset: 0x0033E7E8
		public static float Visibility(float bodySizeSum, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = CaravanVisibilityCalculator.BodySizeSumToVisibility.Evaluate(bodySizeSum);
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("TotalBodySize".Translate() + ": " + bodySizeSum.ToString("0.##"));
			}
			if (!caravanMovingNow)
			{
				num *= 0.3f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 0.3f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x0600B382 RID: 45954 RVA: 0x00074A57 File Offset: 0x00072C57
		public static float Visibility(Caravan caravan, StringBuilder explanation = null)
		{
			return CaravanVisibilityCalculator.Visibility(caravan.PawnsListForReading, caravan.pather.MovingNow, explanation);
		}

		// Token: 0x0600B383 RID: 45955 RVA: 0x0034068C File Offset: 0x0033E88C
		public static float Visibility(List<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < pawns.Count; i++)
			{
				num += pawns[i].BodySize;
			}
			return CaravanVisibilityCalculator.Visibility(num, caravanMovingNow, explanation);
		}

		// Token: 0x0600B384 RID: 45956 RVA: 0x00074A70 File Offset: 0x00072C70
		public static float Visibility(IEnumerable<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			CaravanVisibilityCalculator.tmpPawns.AddRange(pawns);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, caravanMovingNow, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B385 RID: 45957 RVA: 0x003406C8 File Offset: 0x0033E8C8
		public static float Visibility(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B386 RID: 45958 RVA: 0x00340750 File Offset: 0x0033E950
		public static float VisibilityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B387 RID: 45959 RVA: 0x00074A9D File Offset: 0x00072C9D
		public static float VisibilityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CaravanVisibilityCalculator.tmpThingCounts);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpThingCounts, explanation);
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600B388 RID: 45960 RVA: 0x003407E4 File Offset: 0x0033E9E4
		public static float Visibility(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						CaravanVisibilityCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x04007B51 RID: 31569
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04007B52 RID: 31570
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04007B53 RID: 31571
		private static readonly SimpleCurve BodySizeSumToVisibility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.2f),
				true
			},
			{
				new CurvePoint(6f, 1f),
				true
			},
			{
				new CurvePoint(12f, 1.12f),
				true
			}
		};

		// Token: 0x04007B54 RID: 31572
		public const float NotMovingFactor = 0.3f;
	}
}
