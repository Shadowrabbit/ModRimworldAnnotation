using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002100 RID: 8448
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x0600B36E RID: 45934 RVA: 0x0007491E File Offset: 0x00072B1E
		public static int GetTicksPerMove(Caravan caravan, StringBuilder explanation = null)
		{
			if (caravan == null)
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			return CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(caravan), explanation);
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x0007493E File Offset: 0x00072B3E
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, explanation);
		}

		// Token: 0x0600B370 RID: 45936 RVA: 0x00340190 File Offset: 0x0033E390
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, StringBuilder explanation = null)
		{
			if (pawns.Any<Pawn>())
			{
				if (explanation != null)
				{
					explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
				}
				float num = 0f;
				for (int i = 0; i < pawns.Count; i++)
				{
					float num2 = (float)((pawns[i].Downed || pawns[i].CarriedByCaravan()) ? 450 : pawns[i].TicksPerMoveCardinal);
					num2 = Mathf.Min(num2, 150f) * 340f;
					float num3 = 60000f / num2;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append(string.Concat(new string[]
						{
							"  - ",
							pawns[i].LabelShortCap,
							": ",
							num3.ToString("0.#"),
							" "
						}) + "TilesPerDay".Translate());
						if (pawns[i].Downed)
						{
							explanation.Append(" (" + "DownedLower".Translate() + ")");
						}
						else if (pawns[i].CarriedByCaravan())
						{
							explanation.Append(" (" + "Carried".Translate() + ")");
						}
					}
					num += num2 / (float)pawns.Count;
				}
				float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
				if (explanation != null)
				{
					float num4 = 60000f / num;
					explanation.AppendLine();
					explanation.Append("  " + "Average".Translate() + ": " + num4.ToString("0.#") + " " + "TilesPerDay".Translate());
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierForCarriedMass".Translate(moveSpeedFactorFromMass.ToStringPercent()));
				}
				int num5 = Mathf.Max(Mathf.RoundToInt(num / moveSpeedFactorFromMass), 1);
				if (explanation != null)
				{
					float num6 = 60000f / (float)num5;
					explanation.AppendLine();
					explanation.Append("  " + "FinalCaravanPawnsMovementSpeed".Translate() + ": " + num6.ToString("0.#") + " " + "TilesPerDay".Translate());
				}
				return num5;
			}
			if (explanation != null)
			{
				CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
			}
			return 3300;
		}

		// Token: 0x0600B371 RID: 45937 RVA: 0x00340450 File Offset: 0x0033E650
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			if (massCapacity <= 0f)
			{
				return 1f;
			}
			float t = massUsage / massCapacity;
			return Mathf.Lerp(2f, 1f, t);
		}

		// Token: 0x0600B372 RID: 45938 RVA: 0x00340480 File Offset: 0x0033E680
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 18.181818f;
			sb.AppendLine();
			sb.Append("  " + "Default".Translate() + ": " + num.ToString("0.#") + " " + "TilesPerDay".Translate());
		}

		// Token: 0x04007B47 RID: 31559
		private const int MaxPawnTicksPerMove = 150;

		// Token: 0x04007B48 RID: 31560
		private const int DownedPawnMoveTicks = 450;

		// Token: 0x04007B49 RID: 31561
		public const float CellToTilesConversionRatio = 340f;

		// Token: 0x04007B4A RID: 31562
		public const int DefaultTicksPerMove = 3300;

		// Token: 0x04007B4B RID: 31563
		private const float MoveSpeedFactorAtZeroMass = 2f;

		// Token: 0x02002101 RID: 8449
		public struct CaravanInfo
		{
			// Token: 0x0600B373 RID: 45939 RVA: 0x00074958 File Offset: 0x00072B58
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
			}

			// Token: 0x0600B374 RID: 45940 RVA: 0x0007497E File Offset: 0x00072B7E
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
			}

			// Token: 0x04007B4C RID: 31564
			public List<Pawn> pawns;

			// Token: 0x04007B4D RID: 31565
			public float massUsage;

			// Token: 0x04007B4E RID: 31566
			public float massCapacity;
		}
	}
}
