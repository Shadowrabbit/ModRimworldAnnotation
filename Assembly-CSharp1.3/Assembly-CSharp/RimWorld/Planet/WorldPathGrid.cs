using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200172E RID: 5934
	public class WorldPathGrid
	{
		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x060088DA RID: 35034 RVA: 0x00313225 File Offset: 0x00311425
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x060088DB RID: 35035 RVA: 0x00313237 File Offset: 0x00311437
		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		// Token: 0x060088DC RID: 35036 RVA: 0x0031324C File Offset: 0x0031144C
		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		// Token: 0x060088DD RID: 35037 RVA: 0x00313263 File Offset: 0x00311463
		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x060088DE RID: 35038 RVA: 0x00313278 File Offset: 0x00311478
		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x060088DF RID: 35039 RVA: 0x00313298 File Offset: 0x00311498
		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x060088E0 RID: 35040 RVA: 0x003132A9 File Offset: 0x003114A9
		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

		// Token: 0x060088E1 RID: 35041 RVA: 0x003132B3 File Offset: 0x003114B3
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

		// Token: 0x060088E2 RID: 35042 RVA: 0x003132F0 File Offset: 0x003114F0
		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x060088E3 RID: 35043 RVA: 0x00313318 File Offset: 0x00311518
		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

		// Token: 0x060088E4 RID: 35044 RVA: 0x00313348 File Offset: 0x00311548
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

		// Token: 0x060088E5 RID: 35045 RVA: 0x00313468 File Offset: 0x00311668
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

		// Token: 0x060088E6 RID: 35046 RVA: 0x00313570 File Offset: 0x00311770
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

		// Token: 0x060088E7 RID: 35047 RVA: 0x003135AC File Offset: 0x003117AC
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

		// Token: 0x040056EF RID: 22255
		public float[] movementDifficulty;

		// Token: 0x040056F0 RID: 22256
		private int allPathCostsRecalculatedDayOfYear = -1;

		// Token: 0x040056F1 RID: 22257
		private const float ImpassableMovemenetDificulty = 1000f;

		// Token: 0x040056F2 RID: 22258
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x040056F3 RID: 22259
		public const float MaxTempForWinterOffset = 5f;
	}
}
