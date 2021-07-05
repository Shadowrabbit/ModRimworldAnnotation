using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AE RID: 6062
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x06008C89 RID: 35977 RVA: 0x003275CE File Offset: 0x003257CE
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

		// Token: 0x06008C8A RID: 35978 RVA: 0x003275EE File Offset: 0x003257EE
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, explanation);
		}

		// Token: 0x06008C8B RID: 35979 RVA: 0x00327608 File Offset: 0x00325808
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, StringBuilder explanation = null)
		{
			CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Clear();
			if (pawns.Any<Pawn>())
			{
				int num = 0;
				foreach (Pawn pawn in pawns)
				{
					if (pawn.RaceProps.Humanlike)
					{
						num++;
					}
					else if (pawn.IsCaravanRideable())
					{
						CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true));
					}
				}
				float num2 = 1f;
				int num3 = 0;
				int count = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
				if (count > 0 && num > 0)
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Sort();
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Reverse();
					if (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count > num)
					{
						CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.RemoveRange(num, CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count - num);
					}
					num3 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
					while (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count < num)
					{
						CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(1f);
					}
					num2 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Average();
				}
				float num4 = (float)CaravanTicksPerMoveUtility.BaseHumanlikeTicksPerCell() * 340f;
				float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
				int num5 = Mathf.Max(Mathf.RoundToInt(num4 / (moveSpeedFactorFromMass * num2)), 1);
				bool flag = massUsage > massCapacity;
				if (explanation != null)
				{
					float num6 = 60000f / num4;
					explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
					explanation.AppendLine();
					explanation.Append("  " + "StatsReport_BaseValue".Translate() + ": " + num6.ToString("0.#") + " " + "TilesPerDay".Translate());
					explanation.AppendLine();
					explanation.Append("  " + "RideableAnimalsPerPeople".Translate() + string.Format(": {0} / {1}", count, num));
					if (num3 > 0)
					{
						explanation.AppendLine();
						explanation.Append("  " + "MultiplierFromRiddenAnimals".Translate() + ": " + num2.ToStringPercent());
					}
					if (!flag)
					{
						explanation.AppendLine();
						explanation.Append("  " + "MultiplierForCarriedMass".Translate(moveSpeedFactorFromMass.ToStringPercent()));
					}
					float num7 = 60000f / (float)num5;
					explanation.AppendLine();
					explanation.Append("  " + "FinalCaravanPawnsMovementSpeed".Translate() + ": " + num7.ToString("0.#") + " " + "TilesPerDay".Translate());
				}
				return num5;
			}
			if (explanation != null)
			{
				CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
			}
			return 3300;
		}

		// Token: 0x06008C8C RID: 35980 RVA: 0x00327910 File Offset: 0x00325B10
		private static int BaseHumanlikeTicksPerCell()
		{
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 60f;
			return Mathf.RoundToInt(1f / num);
		}

		// Token: 0x06008C8D RID: 35981 RVA: 0x00327940 File Offset: 0x00325B40
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			if (massCapacity <= 0f)
			{
				return 1f;
			}
			float t = massUsage / massCapacity;
			return Mathf.Lerp(2f, 1f, t);
		}

		// Token: 0x06008C8E RID: 35982 RVA: 0x00327970 File Offset: 0x00325B70
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 18.181818f;
			sb.AppendLine();
			sb.Append("  " + "Default".Translate() + ": " + num.ToString("0.#") + " " + "TilesPerDay".Translate());
		}

		// Token: 0x04005926 RID: 22822
		public const float CellToTilesConversionRatio = 340f;

		// Token: 0x04005927 RID: 22823
		public const int DefaultTicksPerMove = 3300;

		// Token: 0x04005928 RID: 22824
		private const float MoveSpeedFactorAtZeroMass = 2f;

		// Token: 0x04005929 RID: 22825
		private static List<float> caravanAnimalSpeedFactors = new List<float>();

		// Token: 0x02002A06 RID: 10758
		public struct CaravanInfo
		{
			// Token: 0x0600E3C2 RID: 58306 RVA: 0x00429F7E File Offset: 0x0042817E
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
			}

			// Token: 0x0600E3C3 RID: 58307 RVA: 0x00429FA4 File Offset: 0x004281A4
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
			}

			// Token: 0x04009DDC RID: 40412
			public List<Pawn> pawns;

			// Token: 0x04009DDD RID: 40413
			public float massUsage;

			// Token: 0x04009DDE RID: 40414
			public float massCapacity;
		}
	}
}
