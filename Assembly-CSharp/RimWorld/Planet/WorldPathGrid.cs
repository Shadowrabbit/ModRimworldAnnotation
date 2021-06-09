using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200200E RID: 8206
	public class WorldPathGrid
	{
		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x0600ADC9 RID: 44489 RVA: 0x000711DA File Offset: 0x0006F3DA
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x0600ADCA RID: 44490 RVA: 0x000711EC File Offset: 0x0006F3EC
		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		// Token: 0x0600ADCB RID: 44491 RVA: 0x00071201 File Offset: 0x0006F401
		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		// Token: 0x0600ADCC RID: 44492 RVA: 0x00071218 File Offset: 0x0006F418
		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x0600ADCD RID: 44493 RVA: 0x0007122D File Offset: 0x0006F42D
		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600ADCE RID: 44494 RVA: 0x0007124D File Offset: 0x0006F44D
		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600ADCF RID: 44495 RVA: 0x0007125E File Offset: 0x0006F45E
		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

		// Token: 0x0600ADD0 RID: 44496 RVA: 0x00071268 File Offset: 0x0006F468
		public void RecalculatePerceivedMovementDifficultyAt(int tile, int? ticksAbs = null)
		{
			if (!Find.WorldGrid.InBounds(tile))
			{
				return;
			}
			bool flag = this.PassableFast(tile);
			this.movementDifficulty[tile] = WorldPathGrid.CalculatedMovementDifficultyAt(tile, true, ticksAbs, null);
			if (flag != this.PassableFast(tile))
			{
				Find.WorldReachability.ClearCache();
			}
		}

		// Token: 0x0600ADD1 RID: 44497 RVA: 0x00329868 File Offset: 0x00327A68
		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x0600ADD2 RID: 44498 RVA: 0x00329890 File Offset: 0x00327A90
		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

		// Token: 0x0600ADD3 RID: 44499 RVA: 0x003298C0 File Offset: 0x00327AC0
		public static float CalculatedMovementDifficultyAt(int tile, bool perceivedStatic, int? ticksAbs = null, StringBuilder explanation = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (explanation != null && explanation.Length > 0)
			{
				explanation.AppendLine();
			}
			if (tile2.biome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				if (explanation != null)
				{
					explanation.Append("Impassable".Translate());
				}
				return 1000f;
			}
			float num = 0f + tile2.biome.movementDifficulty;
			if (explanation != null)
			{
				explanation.Append(tile2.biome.LabelCap + ": " + tile2.biome.movementDifficulty.ToStringWithSign("0.#"));
			}
			float num2 = WorldPathGrid.HillinessMovementDifficultyOffset(tile2.hilliness);
			float num3 = num + num2;
			if (explanation != null && num2 != 0f)
			{
				explanation.AppendLine();
				explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
			}
			return num3 + WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, new int?(ticksAbs ?? GenTicks.TicksAbs), explanation);
		}

		// Token: 0x0600ADD4 RID: 44500 RVA: 0x003299E0 File Offset: 0x00327BE0
		public static float GetCurrentWinterMovementDifficultyOffset(int tile, int? ticksAbs = null, StringBuilder explanation = null)
		{
			if (ticksAbs == null)
			{
				ticksAbs = new int?(GenTicks.TicksAbs);
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			SeasonUtility.GetSeason(GenDate.YearPercent((long)ticksAbs.Value, vector.x), vector.y, out num, out num2, out num3, out num4, out num5, out num6);
			float num7 = num4 + num6;
			num7 *= Mathf.InverseLerp(5f, 0f, GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs.Value, tile));
			if (num7 > 0.01f)
			{
				float num8 = 2f * num7;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("Winter".Translate());
					if (num7 < 0.999f)
					{
						explanation.Append(" (" + num7.ToStringPercent("F0") + ")");
					}
					explanation.Append(": ");
					explanation.Append(num8.ToStringWithSign("0.#"));
				}
				return num8;
			}
			return 0f;
		}

		// Token: 0x0600ADD5 RID: 44501 RVA: 0x00329AE8 File Offset: 0x00327CE8
		public static bool WillWinterEverAffectMovementDifficulty(int tile)
		{
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 3600000; i += 60000)
			{
				if (GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs + i, tile) < 5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600ADD6 RID: 44502 RVA: 0x00329B24 File Offset: 0x00327D24
		private static float HillinessMovementDifficultyOffset(Hilliness hilliness)
		{
			switch (hilliness)
			{
			case Hilliness.Flat:
				return 0f;
			case Hilliness.SmallHills:
				return 0.5f;
			case Hilliness.LargeHills:
				return 1.5f;
			case Hilliness.Mountainous:
				return 3f;
			case Hilliness.Impassable:
				return 1000f;
			default:
				return 0f;
			}
		}

		// Token: 0x04007761 RID: 30561
		public float[] movementDifficulty;

		// Token: 0x04007762 RID: 30562
		private int allPathCostsRecalculatedDayOfYear = -1;

		// Token: 0x04007763 RID: 30563
		private const float ImpassableMovemenetDificulty = 1000f;

		// Token: 0x04007764 RID: 30564
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x04007765 RID: 30565
		public const float MaxTempForWinterOffset = 5f;
	}
}
